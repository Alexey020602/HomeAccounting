using System.Reflection;
using BlazorConsolidated.Common.Attributes;
using BlazorConsolidated.Users.Infrastructure;
using ClientServerShared;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace BlazorConsolidated.DependencyInjection;

public static class RefitDependencyInjectionExtensions 
{

    extension(IServiceCollection serviceCollection)
    {
        // public IServiceCollection AddRefitClients(Uri apiUri)
        // {
        //     serviceCollection.AddRefitClientsFromAssemblies(apiUri, [Assembly.GetCallingAssembly()]);
        //
        //     return serviceCollection;
        // }
        //
        // private void AddRefitClientsFromAssemblies(Uri apiUri, List<Assembly> assemblies)
        // {
        //     foreach (var type in assemblies.SelectMany(a => a.GetTypes()).Where(t => t.IsInterface))
        //     {
        //         var attributes = type.GetCustomAttributes();
        //
        //         foreach (var apiAttribute in attributes.OfType<ApiAttribute>())
        //         {
        //             serviceCollection.AddRefitClient(type, apiUri, apiAttribute);
        //         }
        //     }
        // }

        public IHttpClientBuilder AddBaseRefitClient<T>(Uri apiUri) where T : class
        {
            return serviceCollection.AddRefitClient<T>()
                .ConfigureHttpClient(client => client.BaseAddress = apiUri)
                .AddHttpMessageHandler<HttpLoggingHandler>();
        }

        public IHttpClientBuilder AddHomeAccountingRefitClient<T>(Uri apiUri) where T : class
        {
            return serviceCollection.AddBaseRefitClient<T>(apiUri)
                .AddHttpMessageHandler<AuthenticationHandler>();
        }

        public void AddHomeAccountingRefitClient(Type type, Uri apiUri)
        {
            serviceCollection.AddRefitClient(type)
                .ConfigureHttpClient(client => client.BaseAddress = apiUri)
                .AddHttpMessageHandler<HttpLoggingHandler>()
                .AddHttpMessageHandler<AuthenticationHandler>();
        }

        private void AddRefitClient(Type type, Uri apiUri,
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
                        .AppendingPath(apiAttribute.BasePath)
                )
                .AddHttpMessageHandler<HttpLoggingHandler >();

            if (apiAttribute is not ApiAuthorizableAttribute) return;

            httpClientBuilder
                .AddHttpMessageHandler<AuthenticationHandler>();
        }
    }
}