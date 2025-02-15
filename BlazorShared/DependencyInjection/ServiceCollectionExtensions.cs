using System.Reflection;
using BlazorShared.Api;
using BlazorShared.Api.Attributes;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using Refit;

namespace BlazorShared.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBlazorShared(this IServiceCollection serviceCollection, Uri apiUri)
    {

        serviceCollection.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.VisibleStateDuration = 4000;
        });

        serviceCollection.AddRefitClients(apiUri);
        
        // serviceCollection.AddRefitClient<IChecksApi>()
        //     .ConfigureHttpClient(client => client.BaseAddress = apiUri);
        // serviceCollection.AddRefitClient<IReportsApi>()
        //     .ConfigureHttpClient(client => client.BaseAddress = apiUri);
        
        return serviceCollection;
    }

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
                client.BaseAddress = apiUri.AppendingPath(apiAttribute.BasePath));

        // if (apiAttribute is not  ApiAuthorizableAttribute) continue;
                
        //TODO: добавить MessageHandler для HttpClient
                        
        // httpClientBuilder
        //     .AddHttpMessageHandler<>();
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