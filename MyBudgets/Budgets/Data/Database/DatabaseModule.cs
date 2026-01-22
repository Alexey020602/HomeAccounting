using MyBudgets.Budgets.Data.Database.Seeding;
using MyBudgets.Common.Infrastructure.Database;

namespace MyBudgets.Budgets.Data.Database;

static class DatabaseModule
{
    public static void AddDatabase(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDbContext<BudgetsContext>(
            databaseServiceName,
            optionsAction: options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging()
                        .SetUpBudgetsForDevelopment();
                }
                else
                {
                    options.SetUpBudgets();
                }
            },
            npgsqlOptionsAction: options => options.MigrationsHistoryTable(DbConstants.MigrationTableName, BudgetsContext.ShemaName)
            );
    }
    
}