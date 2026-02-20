using BlazorConsolidated.Budgets;
using BlazorConsolidated.Common;
using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users;
using BlazorConsolidated.Users.Infrastructure;
using BlazorConsolidated.Utils;
using ClientServerContracts.Api.Users;
using ClientServerShared;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;

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
            .AddTransient<AuthenticationHandler>()
            // .AddRefitClients(apiUri)
            .AddAuthorizationModule(apiUri)
            .AddBudgetsModule(apiUri);
}