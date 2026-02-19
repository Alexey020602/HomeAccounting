namespace HomeAccounting.Common.Infrastructure.Events.EventBus.InMemory;

internal interface IInMemoryMessageQueue
{
    IAsyncEnumerable<IIntegrationEvent> ReadEvents(CancellationToken cancellationToken = default);
    ValueTask WriteEvent(IIntegrationEvent @event, CancellationToken cancellationToken = default);
}