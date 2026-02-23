using HomeAccounting.Common.Infrastructure.Events.EventBus.Mediator;

namespace HomeAccounting.Common.Infrastructure.Events.EventBus;

internal static class EventBusModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddEventBus() => services.AddMediatorEventBus();
    }
}