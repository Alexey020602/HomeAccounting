using MyBudgets.Budgets.DeleteBudget;
using MyBudgets.Budgets.GetBudgetDetail;
using MyBudgets.Budgets.GetBudgetSpendings;
using MyBudgets.Budgets.GetBudgetUsers;
using MyBudgets.Budgets.GetUserPermissions;
using MyBudgets.Budgets.GetBudgets;
using MyBudgets.Budgets.UpdateBudget;

namespace MyBudgets.Budgets;

static class BudgetsEndpoints
{
    public static void MapBudgetsEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var budgetsGroup = endpoints.MapGroup("budgets");
        
        budgetsGroup.MapGetBudgets();
        budgetsGroup.MapCreateBudgets();
        budgetsGroup.MapGetBudgetDetails();
        budgetsGroup.MapUpdateBudget();
        budgetsGroup.MapDeleteBudget();
        budgetsGroup.MapGetBudgetUsers();
        budgetsGroup.MapGetUserPermissions();
        budgetsGroup.MapGetBudgetSpendings();
    }
}