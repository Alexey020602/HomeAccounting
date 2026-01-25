using HomeAccounting.Common.Infrastructure.Database;

namespace HomeAccounting.Budgets.Data.Database.Seeding;

static class AutomaticMigrationsExtensions
{
    public static Task MigrateBudgetsAsync(this IHost host, CancellationToken cancellationToken = default) => host.MigrateDatabaseAsync<BudgetsContext>(cancellationToken);
}