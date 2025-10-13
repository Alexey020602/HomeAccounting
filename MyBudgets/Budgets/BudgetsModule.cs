using MyBudgets.Budgets.Data.Database;

namespace MyBudgets.Budgets;

static class BudgetsModule
{
    public static void AddBudgets(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDatabase(databaseServiceName);
        
        
    }
    
}