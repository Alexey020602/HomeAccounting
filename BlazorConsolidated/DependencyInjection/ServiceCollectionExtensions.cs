using System.Reflection;
using BlazorConsolidated.Budgets;
using BlazorConsolidated.Common;
using BlazorConsolidated.Common.Attributes;
using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users;
using BlazorConsolidated.Users.Infrastructure;
using BlazorConsolidated.Users.Infrastructure.Api;
using BlazorConsolidated.Utils;
using ClientServerShared;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Refit;

namespace BlazorConsolidated.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorShared(this IServiceCollection serviceCollection, Uri apiUri) =>
        serviceCollection
            .AddLogging()
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                config.SnackbarConfiguration.PreventDuplicates = false;
                config.SnackbarConfiguration.VisibleStateDuration = 4000;
            })
            .AddDefaultLogoutService()
            .AddTransient<ILocalStorage, LocalStorage>()
            .AddTransient<HttpLoggingHandler>()
            .AddTransient<AuthorizationHandler>()
            .AddRefitClients(apiUri)
            .AddAuthorizationModule()
            .AddBudgetsModule();

    private static IServiceCollection AddRefitClients(this IServiceCollection serviceCollection, Uri apiUri)
    {
        List<Assembly> assemblies =
        [
            Assembly.GetExecutingAssembly(),
            typeof(IAuthorizationApi).Assembly,
            // typeof(IChecksApi).Assembly,
            // typeof(IReportsApi).Assembly,
            // typeof(IBudgetsApi).Assembly
        ];
        foreach (var type in assemblies.SelectMany(a => a.GetTypes()).Where(t => t.IsInterface))
        {
            var attributes = type.GetCustomAttributes();

            foreach (var apiAttribute in attributes.OfType<ApiAttribute>())
            {
                serviceCollection.AddRefitClient(type, apiUri, apiAttribute);
            }
        }

        return serviceCollection;
    }
    private static IServiceCollection AddRefitClient(this IServiceCollection serviceCollection, Type type, Uri apiUri,
        ApiAttribute apiAttribute)
    {
        var jsonSerializerOptions = SystemTextJsonContentSerializer.GetDefaultJsonSerializerOptions();
        
        var jsonContentSerializer = new SystemTextJsonContentSerializer(
            jsonSerializerOptions
        );
        var settings = new RefitSettings
        {
            ContentSerializer = jsonContentSerializer
        };
        var httpClientBuilder = serviceCollection.AddRefitClient(type)
            .ConfigureHttpClient(client =>
                client.BaseAddress = apiUri//.AppendingPath("api", apiAttribute.BasePath)
                    .AppendingPath(Path.Join("api", apiAttribute.BasePath))
                
                
            )
            .AddHttpMessageHandler<HttpLoggingHandler >();

        if (apiAttribute is not ApiAuthorizableAttribute) return serviceCollection;

        httpClientBuilder
            .AddHttpMessageHandler<AuthorizationHandler>();
        return serviceCollection;
    }

    private static Uri AppendingPath(this Uri uri, string? path)
    {
        if (path is null) return uri;

        var uriBuilder = new UriBuilder(uri);
        uriBuilder.Path += path;
        return uriBuilder.Uri;
    }
}