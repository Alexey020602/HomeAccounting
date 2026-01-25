using HomeAccounting.Common.Infrastructure.Database;

namespace HomeAccounting.Categories.Data.DataBase.Seeding;

internal static class AutomaticMigrationsExtensions
{
    extension(IHost host)
    {
        public Task MigrateCategoriesAsync(CancellationToken cancellationToken = default) => host.MigrateDatabaseAsync<CategoriesContext>(cancellationToken);
    }
}