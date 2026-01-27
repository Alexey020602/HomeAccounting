using System.Diagnostics;
using HomeAccounting.Budgets.Events;
using HomeAccounting.Budgets.Services;
using Mediator;
using Microsoft.Extensions.Logging;

namespace HomeAccounting.Budgets.Handlers;

internal sealed class ReceiptProcessingHandler : INotificationHandler<ReceiptCreated>
{
    private readonly ActivitySource activitySource;
    private readonly IReceiptProcessingOrchestrator orchestrator;
    private readonly ILogger<ReceiptProcessingHandler> logger;

    public ReceiptProcessingHandler(
        ActivitySource activitySource,
        IReceiptProcessingOrchestrator orchestrator,
        ILogger<ReceiptProcessingHandler> logger)
    {
        this.activitySource = activitySource;
        this.orchestrator = orchestrator;
        this.logger = logger;
    }

    public async ValueTask Handle(ReceiptCreated notification, CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity("HandleReceiptCreated");
        activity?.SetTag("event.type", "ReceiptCreated");
        activity?.SetTag("receipt.id", notification.ReceiptSpendingId.Value.ToString());
        activity?.SetTag("event.id", notification.Id.ToString());
        activity?.SetTag("event.occurred_at", notification.OccurredDateTime.ToString("O"));
        
        
        logger.LogInformation(
            "Processing receipt creation event {EventId} for receipt {ReceiptId}",
            notification.Id,
            notification.ReceiptSpendingId);

        try
        {
            await orchestrator.ProcessSpendingsByIds([notification.ReceiptSpendingId], cancellationToken);
            
            activity?.SetStatus(ActivityStatusCode.Ok);
            logger.LogInformation(
                "Successfully processed receipt creation event {EventId} for receipt {ReceiptId}",
                notification.Id,
                notification.ReceiptSpendingId);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
            
            logger.LogError(
                ex,
                "Error processing receipt creation event {EventId} for receipt {ReceiptId}: {ExceptionType} - {ErrorMessage}",
                notification.Id,
                notification.ReceiptSpendingId,
                ex.GetType().Name,
                ex.Message);
        }
    }
}
