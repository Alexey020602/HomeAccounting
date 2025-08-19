using Budgets.Core.CreateBudget;
using Budgets.Core.GetBudgets;
using Budgets.DataBase;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Infrastructure;

namespace Budgets.Api;

public static class BudgetsModule
{
    public static void AddBudgetsModule(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder
            .AddDbContext<BudgetsContext>(
                databaseServiceName,
                optionsAction: options => options.SetUpBudgetsForDevelopment(),
                npgsqlOptionsAction: options => options
                    .MigrationsHistoryTable(DbConstants.MigrationTableName, BudgetsDbConstants.SchemaName)
            );

        builder.Services
            .AddScoped<IGetBudgetsService, GetBudgetsService>()
            .AddScoped<ICreateBudgetService, CreateBudgetService>();
    }
}