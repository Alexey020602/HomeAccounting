using MyBudgets.Common.Database;

namespace MyBudgets.Budgets.Data.Database;

static class DatabaseModule
{
    public static void AddDatabase(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDbContext<BudgetsContext>(
            databaseServiceName,
            npgsqlOptionsAction: options => options.MigrationsHistoryTable(DbConstants.MigrationTableName, BudgetsContext.ShemaName)
            );
    }
}