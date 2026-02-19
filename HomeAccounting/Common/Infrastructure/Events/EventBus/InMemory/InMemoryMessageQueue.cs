using System.Threading.Channels;

namespace HomeAccounting.Common.Infrastructure.Events.EventBus.InMemory;

internal sealed class InMemoryMessageQueue : IInMemoryMessageQueue
{
    private readonly Channel<IIntegrationEvent> channel = Channel.CreateUnbounded<IIntegrationEvent>();
    
    public IAsyncEnumerable<IIntegrationEvent> ReadEvents(CancellationToken cancellationToken = default) =>
        channel.Reader.ReadAllAsync(cancellationToken);

    public ValueTask WriteEvent(IIntegrationEvent @event, CancellationToken cancellationToken = default) => 
        channel.Writer.WriteAsync(@event, cancellationToken);
}