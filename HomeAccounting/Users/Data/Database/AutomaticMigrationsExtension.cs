using HomeAccounting.Common.Infrastructure.Database;

namespace HomeAccounting.Users.Data.Database;

static class AutomaticMigrationsExtension
{
    public static Task MigrateUsersAsync(this IHost host, CancellationToken cancellationToken = default)
    {
        return host.MigrateDatabaseAsync<UsersContext>(cancellationToken);
    }
}