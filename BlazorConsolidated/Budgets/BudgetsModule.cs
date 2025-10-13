using BlazorConsolidated.Budgets.BudgetState;
using BlazorConsolidated.Common.Logout;
using ClientServerShared;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Budgets;

public static class BudgetsModule
{
    public static IServiceCollection AddBudgetsModule(this IServiceCollection services) =>
        services
            .AddSingletonAsMultipleServices<IBudgetsStateService, BudgetsStateProvider, BudgetStateService>()
            .AddScoped<ILogoutAction, BudgetsLogoutAction>()
            .AddSingleton<IBudgetStateStorage, BudgetStateStorage>()
            .Decorate<IBudgetStateStorage, TelemetryBudgetStateStorage>()
            .AddCascadingBudgetsState();

    private static IServiceCollection AddCascadingBudgetsState(this IServiceCollection services) =>
        services
            .AddCascadingValue/*<Task<BudgetState.BudgetState>>*/( sourceFactory: services =>
            new BudgetCascadingValueSource(services.GetRequiredService<BudgetsStateProvider>())
        );
}