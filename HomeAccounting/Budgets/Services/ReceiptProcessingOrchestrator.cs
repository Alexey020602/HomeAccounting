using System.Diagnostics;
using System.Linq.Expressions;
using ClientServerShared.Model.Money;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Configuration;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.ReceiptProcessing.Contracts;
using HomeAccounting.ReceiptProcessing.GetReceiptData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HomeAccounting.Budgets.Services;

internal sealed class ReceiptProcessingOrchestrator : IReceiptProcessingOrchestrator
{
    private readonly ActivitySource activitySource;
    private readonly IReceiptProcessService receiptProcessService;
    private readonly ReceiptProcessingOptions options;
    private readonly BudgetsContext budgetsContext;
    private readonly ILogger<ReceiptProcessingOrchestrator> logger;

    public ReceiptProcessingOrchestrator(
        ActivitySource activitySource,
        IReceiptProcessService receiptProcessService,
        IOptions<ReceiptProcessingOptions> options,
        BudgetsContext budgetsContext,
        ILogger<ReceiptProcessingOrchestrator> logger)
    {
        this.activitySource = activitySource;
        this.receiptProcessService = receiptProcessService;
        this.options = options.Value;
        this.budgetsContext = budgetsContext;
        this.logger = logger;
    }

    public async Task ProcessEnableToRetries(CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();
        
        var now = DateTime.UtcNow;

        logger.LogInformation("Starting processing of receipts ready for retry");

        var receiptsToRetry = await budgetsContext.Budgets
            .SelectMany(b => b.Spendings)
            .OfType<ReceiptSpending>()
            .Where(CanRetryFilter(options.MaxRetries, now))
            .ToListAsync(cancellationToken);
        
        await ProcessSpendings(receiptsToRetry, cancellationToken);
        
        await budgetsContext.SaveChangesAsync(cancellationToken);

        
        logger.LogInformation(
            "Completed processing {Count} receipts",
            receiptsToRetry.Count);
    }
    public async Task ProcessSpendingsByIds(SpendingId[] ids, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();
        
        logger.LogInformation("Starting processing of receipts by IDs");

        var spendings = await budgetsContext.Budgets
            .SelectMany(b => b.Spendings)
            .Where(s => ids.Contains(s.Id))
            .OfType<ReceiptSpending>()
            .ToListAsync(cancellationToken);

        var foundCount = spendings.Count;

        if (foundCount != ids.Length)
        {
            logger.LogWarning(
                "Expected {ExpectedCount} receipts, but found {FoundCount}",
                ids.Length,
                foundCount);
        }
        
        await ProcessSpendings(spendings, cancellationToken);

        await budgetsContext.SaveChangesAsync(cancellationToken);

        logger.LogInformation(
            "Completed processing {Count} receipts by IDs",
            foundCount);
    }

    private async Task ProcessSpendings(List<ReceiptSpending> spendings, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();

        var spendingsCount = spendings.Count;
        logger.LogDebug("Processing {Count} receipts in parallel", spendingsCount);

        var tasks = spendings.Select(spending => ProcessReceiptSpending(spending, cancellationToken));

        await Task.WhenAll(tasks);
        
        var successCount = spendings.Count(s => s.Status == ReceiptProcessingStatus.Succeeded);
        var failedCount = spendings.Count(s => s.Status == ReceiptProcessingStatus.Failed);
        var processingCount = spendings.Count(s => s.Status == ReceiptProcessingStatus.Processing);
        
        activity?.SetTag("receipts.success", successCount);
        activity?.SetTag("receipts.failed", failedCount);
        activity?.SetTag("receipts.processing", processingCount);
        
        logger.LogInformation(
            "Processed {TotalCount} receipts: {SuccessCount} succeeded, {FailedCount} failed, {ProcessingCount} still processing",
            spendings.Count,
            successCount,
            failedCount,
            processingCount);
    }

    private async Task ProcessReceiptSpending(ReceiptSpending spending, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();
        activity?.SetTag("receipt.id", spending.Id.Value.ToString());
        activity?.SetTag("receipt.fiscal_data.fn", spending.FiscalData.Fn);
        activity?.SetTag("receipt.fiscal_data.fd", spending.FiscalData.Fd);
        activity?.SetTag("receipt.fiscal_data.fp", spending.FiscalData.Fp);
        
        var attemptedAt = DateTime.UtcNow;
        var startTime = DateTime.UtcNow;
        var retryCount = spending.Attempts.Count(a => !a.IsSuccess);
        
        activity?.SetTag("retry.count", retryCount);
        
        logger.LogDebug(
            "Processing receipt {ReceiptId} (attempt {RetryCount})",
            spending.Id,
            retryCount + 1);

        var result = await ProcessReceiptAsync(spending, cancellationToken);
        
        var duration = DateTime.UtcNow - startTime;
        activity?.SetTag("duration_ms", duration.TotalMilliseconds);

        if (result.IsSuccess)
        {
            var purchasePlace = result.PurchasePlace ?? throw new InvalidOperationException("Purchase place not found");
            var products = result.Products ?? throw new InvalidOperationException("Product not found");
            var productsCount = products.Count();
            
            spending.MarkProcessingSucceeded(attemptedAt, purchasePlace, products);
            
            logger.LogInformation(
                "Successfully processed receipt {ReceiptId} with {ProductsCount} products in {DurationMs}ms",
                spending.Id,
                productsCount,
                duration.TotalMilliseconds);
            
            return;
        }

        var errorMessage = result.ErrorMessage ?? throw new InvalidOperationException("Error message not found");
        
        if (result.IsRetryable)
        {
            var nextRetryAt = result.NextRetryAt ?? throw new InvalidOperationException("Next retry at not found");
            
            spending.MarkProcessingRetryableError(attemptedAt, nextRetryAt, errorMessage);
            
            logger.LogWarning(
                "Retryable error processing receipt {ReceiptId} (attempt {RetryCount}): {ErrorMessage}. Next retry at {NextRetryAt}",
                spending.Id,
                retryCount + 1,
                errorMessage,
                nextRetryAt);
            
            return;
        }
        
        activity?.SetTag("result", "terminal_error");
        spending.MarkProcessingFailed(attemptedAt, errorMessage);
        
        logger.LogError(
            "Terminal error processing receipt {ReceiptId} (attempt {RetryCount}): {ErrorMessage}",
            spending.Id,
            retryCount + 1,
            errorMessage);
    }
    private async Task<ProcessingResult> ProcessReceiptAsync(
        ReceiptSpending receiptSpending,
        CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity("ProcessReceipt");
        activity?.SetTag("receipt.id", receiptSpending.Id.Value.ToString());
        
        try
        {
            var request = new GetReceiptDataRequest(receiptSpending.FiscalData);
            var response = await receiptProcessService.GetReceiptData(request, cancellationToken);

            var products = response.Products
                .Select(p => new ProductInput(
                    p.Name,
                    p.Quantity,
                    Money.FromKopecks(p.Price),
                    Money.FromKopecks(p.Sum),
                    null))
                .ToList();
            
            logger.LogDebug(
                "Successfully received receipt data for {ReceiptId}: {ProductsCount} products, place: {PurchasePlace}",
                receiptSpending.Id,
                products.Count,
                response.PurchasePlase);

            return ProcessingResult.Success(response.PurchasePlase, products);
        }
        catch (ReceiptProcessException ex)
        {
            
            if (!IsRetryable(ex))
            {
                activity?.AddException(ex);
                
                logger.LogWarning(
                    ex,
                    "Terminal error processing receipt {ReceiptId}: {ErrorMessage}",
                    receiptSpending.Id,
                    ex.Message);

                return ProcessingResult.TerminalError(ex.Message);
            }
            
            var retryCount = receiptSpending.Attempts.Count(a => !a.IsSuccess);
            var nextRetryAt = CalculateNextRetryAt(retryCount);

            activity?.SetStatus(ActivityStatusCode.Ok);
                
            logger.LogWarning(
                "Retryable error processing receipt {ReceiptId}: {ErrorMessage}. Retry {RetryCount}, next retry at {NextRetryAt}",
                receiptSpending.Id,
                ex.Message,
                retryCount + 1,
                nextRetryAt);

            return ProcessingResult.RetryableError(ex.Message, nextRetryAt);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);

            logger.LogError(
                ex,
                "Unexpected error processing receipt {ReceiptId}: {ErrorMessage}",
                receiptSpending.Id,
                ex.Message);
            
            return ProcessingResult.TerminalError($"Unexpected error: {ex.Message}");
        }
    }

    private static bool IsRetryable(ReceiptProcessException ex)
    {
        return ex is DataNotReceivedYetProcessException
            or NumberOfRequestsExceededProcessException
            or WaitingBeforeRepeatRequestProcessException;
    }

    private DateTime CalculateNextRetryAt(int retryCount)
    {
        var delay = CalculateDelay(retryCount);
        return DateTime.UtcNow.Add(delay);
    }

    private TimeSpan CalculateDelay(int retryCount)
    {
        var delay = TimeSpan.FromMilliseconds(
            options.InitialDelay.TotalMilliseconds * Math.Pow(options.BackoffMultiplier, retryCount));

        return delay > options.MaxDelay ? options.MaxDelay : delay;
    }

    private static Expression<Func<ReceiptSpending, bool>> CanRetryFilter(int maxRetries, DateTime now) => receipt => 
        (receipt.Status == ReceiptProcessingStatus.Processing) && 
        (receipt.Attempts.Count(a=>!a.IsSuccess) < maxRetries) &&
        (receipt.NextRetryAt == null || receipt.NextRetryAt < now);
}

internal sealed record ProcessingResult
{
    public bool IsSuccess { get; init; }
    public bool IsRetryable { get; init; }
    public string? ErrorMessage { get; init; }
    public DateTime? NextRetryAt { get; init; }
    public string? PurchasePlace { get; init; }
    public IEnumerable<ProductInput>? Products { get; init; }

    public static ProcessingResult Success(string purchasePlace, IEnumerable<ProductInput> products) =>
        new()
        {
            IsSuccess = true,
            PurchasePlace = purchasePlace,
            Products = products
        };

    public static ProcessingResult RetryableError(string errorMessage, DateTime nextRetryAt) =>
        new()
        {
            IsRetryable = true,
            ErrorMessage = errorMessage,
            NextRetryAt = nextRetryAt
        };

    public static ProcessingResult TerminalError(string errorMessage) =>
        new()
        {
            ErrorMessage = errorMessage,
        };
}
