using Mediator;

namespace MyBudgets.Common.Infrastructure.Events.EventBus.Mediator;

internal sealed class MediatorEventBus(IMediator mediator) : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IIntegrationEvent
    {
        await mediator.Publish(@event, cancellationToken);
    }
}