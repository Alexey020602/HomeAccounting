using System.Diagnostics;
using HomeAccounting.Common.Infrastructure.Events.EventBus.InMemory;
using Mediator;

namespace HomeAccounting.Common.Infrastructure.Events.EventBus.Mediator;

internal sealed class IntegrationEventsProcessingJob(
    IInMemoryMessageQueue inMemoryMessageQueue,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<IntegrationEventsProcessingJob> logger,
    ActivitySource activitySource
    ): BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
       await foreach(var @event in inMemoryMessageQueue.ReadEvents(stoppingToken))
       {
           await ProcessEvent(@event, stoppingToken);
       }
    }

    private async Task ProcessEvent(IIntegrationEvent @event, CancellationToken stoppingToken)
    {
        using var activity = activitySource.StartActivity();
        try
        {
            await using var scope = serviceScopeFactory.CreateAsyncScope();
            var publisher = scope.ServiceProvider.GetRequiredService<IPublisher>();
            await publisher.Publish(@event, stoppingToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Something went wrong! {IntegrationEventId}",
                @event.Id);
        }
    }
}