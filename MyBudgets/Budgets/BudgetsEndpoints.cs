using MyBudgets.Budgets.GetBudgets;

namespace MyBudgets.Budgets;

static class BudgetsEndpoints
{
    public static void MapBudgetsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var budgetsGroup = endpoints.MapGroup("budgets");
        
        budgetsGroup.MapGetBudgets();
    }
}