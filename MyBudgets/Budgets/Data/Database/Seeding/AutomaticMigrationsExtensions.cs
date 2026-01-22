using MyBudgets.Common.Infrastructure.Database;

namespace MyBudgets.Budgets.Data.Database.Seeding;

static class AutomaticMigrationsExtensions
{
    public static Task MigrateBudgetsAsync(this IHost host, CancellationToken cancellationToken = default) => host.MigrateDatabaseAsync<BudgetsContext>(cancellationToken);
}