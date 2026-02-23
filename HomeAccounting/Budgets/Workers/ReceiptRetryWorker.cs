using System.Diagnostics;
using HomeAccounting.Budgets.Data;
using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Budgets.Services;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Budgets.Workers;

internal sealed class ReceiptRetryWorker : BackgroundService
{
    private readonly ActivitySource activitySource;
    private readonly IServiceProvider serviceProvider;
    private readonly ILogger<ReceiptRetryWorker> logger;
    private readonly TimeSpan pollingInterval = TimeSpan.FromSeconds(30);

    public ReceiptRetryWorker(
        ActivitySource activitySource,
        IServiceProvider serviceProvider,
        ILogger<ReceiptRetryWorker> logger)
    {
        this.activitySource = activitySource;
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Receipt retry worker started. Polling interval: {IntervalSeconds}s", pollingInterval.TotalSeconds);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            using var activity = activitySource.StartActivity("ReceiptRetryWorkerCycle");
            
            try
            {
                await ProcessRetriesAsync(stoppingToken);

                activity?.SetStatus(ActivityStatusCode.Ok);
                
                logger.LogDebug(
                    "Receipt retry worker cycle completed");
            }
            catch (Exception ex)
            {
                activity?.AddException(ex);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                logger.LogError(
                    ex,
                    "Error in receipt retry worker cycle: {ExceptionType} - {ErrorMessage}",
                    ex.GetType().Name,
                    ex.Message);
            }

            await Task.Delay(pollingInterval, stoppingToken);
        }
        
        logger.LogInformation("Receipt retry worker stopped");
    }

    private async Task ProcessRetriesAsync(CancellationToken cancellationToken)
    {
        using var activity = activitySource.StartActivity("ProcessRetries");
        
        
        logger.LogDebug("Starting receipt retry processing cycle");

        try
        {
            using var scope = serviceProvider.CreateScope();
            var orchestrator = scope.ServiceProvider.GetRequiredService<IReceiptProcessingOrchestrator>();

            await orchestrator.ProcessEnableToRetries(cancellationToken);
            
            
            logger.LogDebug(
                "Completed receipt retry processing cycle");
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);
            
            logger.LogError(
                ex,
                "Error in receipt retry processing cycle: {ExceptionType} - {ErrorMessage}",
                ex.GetType().Name,
                ex.Message);
            
            throw;
        }
    }
}
