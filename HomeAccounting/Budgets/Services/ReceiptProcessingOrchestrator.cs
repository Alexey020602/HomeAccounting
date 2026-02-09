using System.Diagnostics;
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

        var now = DateTimeOffset.UtcNow;
        logger.LogInformation("Starting processing of receipts ready for retry");

        var outboxEntries = await budgetsContext.ReceiptProcessingOutbox
            .Where(e => e.Status == ReceiptProcessingOutboxStatus.Pending)
            .Where(e => e.AttemptCount < options.MaxRetries)
            .Where(e => e.NextRetryAt == null || e.NextRetryAt <= now)
            .ToListAsync(cancellationToken);

        if (outboxEntries.Count == 0)
        {
            logger.LogDebug("No receipts ready for retry");
            return;
        }

        var receiptIds = outboxEntries.Select(e => e.ReceiptId).Distinct().ToArray();
        var receipts = await budgetsContext.Receipts
            .Where(r => receiptIds.Contains(r.Id))
            .ToDictionaryAsync(r => r.Id, cancellationToken);

        foreach (var outboxEntry in outboxEntries)
        {
            if (!receipts.TryGetValue(outboxEntry.ReceiptId, out var receipt))
            {
                logger.LogWarning("Receipt {ReceiptId} not found for outbox entry", outboxEntry.ReceiptId);
                continue;
            }
            await ProcessReceipt(receipt, outboxEntry, cancellationToken);
        }

        await budgetsContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Completed processing {Count} receipts for retry", outboxEntries.Count);
    }

    public async Task ProcessReceiptsByIds(ReceiptId[] ids, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();
        logger.LogInformation("Starting processing of receipts by IDs");

        var receipts = await budgetsContext.Receipts
            .Where(r => ids.Contains(r.Id))
            .ToListAsync(cancellationToken);

        var outboxEntries = await budgetsContext.ReceiptProcessingOutbox
            .Where(e => ids.Contains(e.ReceiptId))
            .ToDictionaryAsync(e => e.ReceiptId, cancellationToken);

        foreach (var receipt in receipts)
        {
            if (!outboxEntries.TryGetValue(receipt.Id, out var outboxEntry))
            {
                outboxEntry = ReceiptProcessingOutboxEntry.Create(receipt.Id);
                budgetsContext.ReceiptProcessingOutbox.Add(outboxEntry);
            }
            await ProcessReceipt(receipt, outboxEntry, cancellationToken);
        }

        await budgetsContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Completed processing {Count} receipts by IDs", receipts.Count);
    }

    private async Task ProcessReceipt(Receipt receipt, ReceiptProcessingOutboxEntry outboxEntry, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity();
        activity?.SetTag("receipt.id", receipt.Id.Value.ToString());

        var attemptedAt = DateTimeOffset.UtcNow;
        var retryCount = outboxEntry.AttemptCount;

        logger.LogDebug("Processing receipt {ReceiptId} (attempt {RetryCount})", receipt.Id, retryCount + 1);

        var result = await ProcessReceiptAsync(receipt, outboxEntry, cancellationToken);

        if (result.IsSuccess)
        {
            var purchasePlace = result.PurchasePlace ?? throw new InvalidOperationException("Purchase place not found");
            var products = result.Products ?? throw new InvalidOperationException("Product not found");
            receipt.MarkProcessingSucceeded(attemptedAt, purchasePlace, products);
            outboxEntry.MarkCompleted();
            logger.LogInformation("Successfully processed receipt {ReceiptId} with {ProductsCount} products", receipt.Id, products.Count());
            return;
        }

        var errorMessage = result.ErrorMessage ?? throw new InvalidOperationException("Error message not found");

        if (result.IsRetryable)
        {
            var nextRetryAt = result.NextRetryAt ?? throw new InvalidOperationException("Next retry at not found");
            outboxEntry.ScheduleRetry(nextRetryAt, errorMessage);
            logger.LogWarning("Retryable error processing receipt {ReceiptId} (attempt {RetryCount}): {ErrorMessage}. Next retry at {NextRetryAt}",
                receipt.Id, retryCount + 1, errorMessage, nextRetryAt);
            return;
        }

        receipt.MarkProcessingFailed(attemptedAt, errorMessage);
        outboxEntry.MarkFailed();
        logger.LogError("Terminal error processing receipt {ReceiptId} (attempt {RetryCount}): {ErrorMessage}", receipt.Id, retryCount + 1, errorMessage);
    }

    private async Task<ProcessingResult> ProcessReceiptAsync(Receipt receipt, ReceiptProcessingOutboxEntry outboxEntry, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity("ProcessReceipt");
        activity?.SetTag("receipt.id", receipt.Id.Value.ToString());

        try
        {
            var request = new GetReceiptDataRequest(receipt.FiscalData);
            var response = await receiptProcessService.GetReceiptData(request, cancellationToken);

            var products = response.Products
                .Select(p => new ProductInput(
                    p.Name,
                    p.Quantity,
                    Money.FromKopecks(p.Price),
                    Money.FromKopecks(p.Sum),
                    null))
                .ToList();

            logger.LogDebug("Successfully received receipt data for {ReceiptId}: {ProductsCount} products, place: {PurchasePlace}",
                receipt.Id, products.Count, response.PurchasePlase);

            return ProcessingResult.Success(response.PurchasePlase, products);
        }
        catch (ReceiptProcessException ex)
        {
            if (!IsRetryable(ex))
            {
                activity?.AddException(ex);
                logger.LogWarning(ex, "Terminal error processing receipt {ReceiptId}: {ErrorMessage}", receipt.Id, ex.Message);
                return ProcessingResult.TerminalError(ex.Message);
            }

            var nextRetryAt = CalculateNextRetryAt(outboxEntry.AttemptCount);
            logger.LogWarning("Retryable error processing receipt {ReceiptId}: {ErrorMessage}. Next retry at {NextRetryAt}", receipt.Id, ex.Message, nextRetryAt);
            return ProcessingResult.RetryableError(ex.Message, nextRetryAt);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error);
            activity?.AddException(ex);
            logger.LogError(ex, "Unexpected error processing receipt {ReceiptId}: {ErrorMessage}", receipt.Id, ex.Message);
            return ProcessingResult.TerminalError($"Unexpected error: {ex.Message}");
        }
    }

    private static bool IsRetryable(ReceiptProcessException ex)
    {
        return ex is DataNotReceivedYetProcessException
            or NumberOfRequestsExceededProcessException
            or WaitingBeforeRepeatRequestProcessException;
    }

    private DateTimeOffset CalculateNextRetryAt(int retryCount)
    {
        var delay = CalculateDelay(retryCount);
        return DateTimeOffset.UtcNow.Add(delay);
    }

    private TimeSpan CalculateDelay(int retryCount)
    {
        var delay = TimeSpan.FromMilliseconds(
            options.InitialDelay.TotalMilliseconds * Math.Pow(options.BackoffMultiplier, retryCount));
        return delay > options.MaxDelay ? options.MaxDelay : delay;
    }
}

internal sealed record ProcessingResult
{
    public bool IsSuccess { get; init; }
    public bool IsRetryable { get; init; }
    public string? ErrorMessage { get; init; }
    public DateTimeOffset? NextRetryAt { get; init; }
    public string? PurchasePlace { get; init; }
    public IEnumerable<ProductInput>? Products { get; init; }

    public static ProcessingResult Success(string purchasePlace, IEnumerable<ProductInput> products) =>
        new() { IsSuccess = true, PurchasePlace = purchasePlace, Products = products };

    public static ProcessingResult RetryableError(string errorMessage, DateTimeOffset nextRetryAt) =>
        new() { IsRetryable = true, ErrorMessage = errorMessage, NextRetryAt = nextRetryAt };

    public static ProcessingResult TerminalError(string errorMessage) =>
        new() { ErrorMessage = errorMessage };
}
