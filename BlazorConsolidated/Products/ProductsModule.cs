using BlazorConsolidated.DependencyInjection;
using ClientServerContracts.Api.Products;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Products;

public static class ProductsModule
{
    public static IServiceCollection AddProductsModule(this IServiceCollection services, Uri apiUri)
    {
        services.AddHomeAccountingRefitClient<IProductsApi>(apiUri);
        return services;
    }
}
