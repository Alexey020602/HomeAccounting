using HomeAccounting.Common.Infrastructure.Events.EventBus.InMemory;

namespace HomeAccounting.Common.Infrastructure.Events.EventBus.Mediator;

internal static class MediatorEventBusModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMediatorEventBus()
        {
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            services.AddSingleton<IInMemoryMessageQueue, InMemoryMessageQueue>();
            
            services.AddMediator(options =>
            {
                options.GenerateTypesAsInternal = true;
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });

            services.AddHostedService<IntegrationEventsProcessingJob>();

            return services;
        }
    } 
}