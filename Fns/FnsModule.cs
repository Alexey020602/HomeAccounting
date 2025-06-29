using Fns.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Shared.Utils;

namespace Fns;

public static class FnsModule
{
    public static IServiceCollection AddFnsModule(this IServiceCollection services)
    {
        services.AddScoped<HttpLoggingHandler>();
        services.AddRefitClient<ICheckService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"))
            .AddHttpMessageHandler<HttpLoggingHandler>();
        services.AddRefitClient<IReceiptService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"))
            .AddHttpMessageHandler<HttpLoggingHandler>();
        
        services.AddScoped<ICheckSource, CheckSource>();//todo Убрать, когда окончательно переделаю запросы

        services.AddScoped<IProductCategorizationService, ProductCategorizationService>();
        services.AddScoped<IReceiptDataService, ReceiptDataService>()
            .Decorate<IReceiptDataService, TelemetryReceiptDataService>();
        return services;
        
    }
}