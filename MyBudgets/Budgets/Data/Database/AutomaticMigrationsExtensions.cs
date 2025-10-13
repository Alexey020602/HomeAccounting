using MyBudgets.Common.Database;

namespace MyBudgets.Budgets.Data.Database;

static class AutomaticMigrationsExtensions
{
    public static Task MigrateBudgetsAsync(this IHost host, CancellationToken cancellationToken = default) => host.MigrateDatabaseAsync<BudgetsContext>(cancellationToken);
}