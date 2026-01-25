namespace HomeAccounting.Common.Infrastructure.Events.EventBus.Mediator;

internal static class MediatorEventBusModule
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddMediatorEventBus()
        {
            services.AddScoped<IEventBus, MediatorEventBus>();
            services.AddMediator(options =>
            {
                options.GenerateTypesAsInternal = true;
                options.ServiceLifetime = ServiceLifetime.Scoped;
            });

            return services;
        }
    } 
}