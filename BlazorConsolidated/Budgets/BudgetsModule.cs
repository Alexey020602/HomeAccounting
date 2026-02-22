using BlazorConsolidated.Budgets.BudgetState;
using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.DependencyInjection;
using ClientServerContracts.Api.Budgets;
using ClientServerShared;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Budgets;

public static class BudgetsModule
{
    public static IServiceCollection AddBudgetsModule(this IServiceCollection services, Uri apiUri)
    {
        services
            .AddSingletonAsMultipleServices<IBudgetsStateService, BudgetsStateProvider, BudgetStateService>()
            .AddSingleton<ILogoutAction, BudgetsLogoutAction>()
            .AddSingleton<IBudgetStateStorage, BudgetStateStorage>()
            .Decorate<IBudgetStateStorage, TelemetryBudgetStateStorage>()
            .AddCascadingBudgetsState();
        
        services.AddHomeAccountingRefitClient<IBudgetsApi>(apiUri);

        return services;
    }

    private static IServiceCollection AddCascadingBudgetsState(this IServiceCollection services) =>
        services
            .AddCascadingValue/*<Task<BudgetState.BudgetState>>*/( sourceFactory: services =>
            new BudgetCascadingValueSource(services.GetRequiredService<BudgetsStateProvider>())
        );
}