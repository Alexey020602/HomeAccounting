using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri("https://proverkacheka.com");
            });


        services.AddScoped<IReceiptDataService, ReceiptDataService>()
            .Decorate<IReceiptDataService, TelemetryReceiptDataService>();
        return services;
    }

    public static IServiceCollection AddCategorization(this IServiceCollection services)
    {
        
        var settings = new RefitSettings()
        {
            HttpMessageHandlerFactory = ReceiptCategorizationHttpHandlerFactory
        };
        services.AddRefitClient<IReceiptService>(settings)
            .ConfigureHttpClient(c =>
            { 
                c.BaseAddress = new Uri("https://cheicheck.ru");
            });
        services.AddScoped<IProductCategorizationService, ProductCategorizationService>()
            .Decorate<IProductCategorizationService, TelemetryProductCategorizationService>();
        return services;
    }

    private static HttpClientHandler ReceiptCategorizationHttpHandlerFactory() => new ()
    {
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            
            if (
                sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors
                && CheckCertificateChainIsNotTimeValidOnly(chain))
            {
                return true;
            }

            if (chain is null || cert is null)
            {
                return false;
            }

            return chain.Build(cert);
        }
    };

    private static bool CheckCertificateChainIsNotTimeValidOnly(X509Chain? chain) => 
        chain is not null
        && chain.ChainStatus.Length == 1
        && chain.ChainStatus.FirstOrDefault() is { } chainStatus
        && (chainStatus.Status & X509ChainStatusFlags.NotTimeValid) ==
        X509ChainStatusFlags.NotTimeValid;
}