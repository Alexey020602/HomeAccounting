using MyBudgets.Common.Database;

namespace MyBudgets.Users.Data.Database;

static class AutomaticMigrationsExtension
{
    public static Task MigrateUsersAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        return host.MigrateDatabaseAsync<UsersContext>(cancellationToken);
    }
}