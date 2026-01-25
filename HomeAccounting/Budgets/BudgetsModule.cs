using Microsoft.AspNetCore.Authorization;
using HomeAccounting.Budgets.Data.Database;

namespace HomeAccounting.Budgets;

internal static class BudgetsModule
{
    public static void AddBudgets(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDatabase(databaseServiceName);

        builder.Services.AddScoped<IAuthorizationHandler, BudgetRequirementsAuthorizationHandler>();
    }
    
}