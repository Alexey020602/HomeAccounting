using System.Reflection;
using Authorization.UI;
using Authorization.UI.AuthenticationStateProvider;
using BlazorConsolidated.Services;
using BlazorConsolidated.Utils;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Refit;
using Shared.Blazor;
using Shared.Blazor.Attributes;
using Shared.Utils;

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
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            // .AddSingleton<IAuthenticationStorage, MemoryAuthenticationStorage>()
            .AddTransient<ILocalStorage, LocalStorage>()
            .AddScoped<HttpLoggingHandler>()
            .AddTransient<AuthorizationHandler>()
            .AddRefitClients(apiUri)
            .AddAuthorizationState();

    private static IServiceCollection AddAuthorizationState(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddAuthorizationCore()
            .AddCascadingAuthenticationState()
            .AddScoped<StorageLoginStateProvider>()
            .AddTransient<ILoginService, LoginService>()
            .AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<StorageLoginStateProvider>())
            .AddScoped<ILoginStateProvider>(sp => sp.GetRequiredService<StorageLoginStateProvider>())
        ;

    private static IServiceCollection AddRefitClients(this IServiceCollection serviceCollection, Uri apiUri)
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsInterface))
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
        var httpClientBuilder = serviceCollection.AddRefitClient(type)
            .ConfigureHttpClient(client =>
                client.BaseAddress = apiUri//.AppendingPath("api", apiAttribute.BasePath)
                    .AppendingPath($"api/{apiAttribute.BasePath}")
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