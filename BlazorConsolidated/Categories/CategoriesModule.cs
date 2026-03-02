using BlazorConsolidated.DependencyInjection;
using ClientServerContracts.Api.Categories;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Categories;

public static class CategoriesModule
{
    public static IServiceCollection AddCategoriesModule(this IServiceCollection services, Uri apiUri)
    {
        services.AddHomeAccountingRefitClient<ICategoriesApi>(apiUri);
        return services;
    }
}
