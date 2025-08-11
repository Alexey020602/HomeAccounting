using Budgets.UI.BudgetState;
using Microsoft.Extensions.DependencyInjection;
using Shared.Utils;

namespace Budgets.UI;

public static class BudgetsModule
{
    public static IServiceCollection AddBudgetsModule(this IServiceCollection services) =>
        services
            .AddSingletonAsMultipleServices<IBudgetsStateService, BudgetsStateProvider, BudgetStateService>()
            .AddSingleton<IBudgetStateStorage, BudgetStateStorage>()
            .Decorate<IBudgetStateStorage, TelemetryBudgetStateStorage>()
            .AddCascadingBudgetsState();

    private static IServiceCollection AddCascadingBudgetsState(this IServiceCollection services) =>
        services.AddCascadingValue(services =>
            new BudgetCascadingValueSource(services.GetRequiredService<BudgetsStateProvider>()));
}