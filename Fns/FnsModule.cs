using Fns.Categorization;
using Fns.Contracts;
using Fns.Contracts.Categorization;
using Fns.Contracts.ReceiptData;
using Fns.ReceiptData;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Shared.Utils;

namespace Fns;

public static class FnsModule
{
    public static IServiceCollection AddFnsModule(this IServiceCollection services)
    {
        return services.AddReceiptData().AddCategorization();
    }

    private static IServiceCollection AddReceiptData(this IServiceCollection services)
    {
        services.AddRefitClient<ICheckService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://proverkacheka.com"));


        services.AddScoped<IReceiptDataService, ReceiptDataService>()
            .Decorate<IReceiptDataService, TelemetryReceiptDataService>();
        return services;
    }

    public static IServiceCollection AddCategorization(this IServiceCollection services)
    {
        services.AddRefitClient<IReceiptService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://cheicheck.ru"));
        services.AddScoped<IProductCategorizationService, ProductCategorizationService>();
        return services;
    } 
}