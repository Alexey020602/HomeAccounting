namespace HomeAccounting.Common.Infrastructure.Events.EventBus.InMemory;

internal sealed class InMemoryEventBus(IInMemoryMessageQueue messageQueue): IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent
    {
        await messageQueue.WriteEvent(@event, cancellationToken);
    }
}