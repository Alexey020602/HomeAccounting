using HomeAccounting.Budgets.AddManualSpending;
using HomeAccounting.Budgets.AddReceiptFromFile;
using HomeAccounting.Budgets.AddReceiptSpending;
using HomeAccounting.Budgets.AddUsersInBudget;
using HomeAccounting.Budgets.ChangeReceiptProductCategory;
using HomeAccounting.Budgets.CreateBudget;
using HomeAccounting.Budgets.DeleteReceiptProductCategory;
using HomeAccounting.Budgets.DeleteBudget;
using HomeAccounting.Budgets.DeleteUserFromBudget;
using HomeAccounting.Budgets.GetBudgetDetail;
using HomeAccounting.Budgets.GetBudgetSpendings;
using HomeAccounting.Budgets.GetBudgetUsers;
using HomeAccounting.Budgets.GetOperations;
using HomeAccounting.Budgets.GetUserPermissions;
using HomeAccounting.Budgets.GetBudgets;
using HomeAccounting.Budgets.UpdateBudget;

namespace HomeAccounting.Budgets;

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
        budgetsGroup.MapAddUsersInBudget();
        budgetsGroup.MapDeleteUserFromBudget();
        budgetsGroup.MapGetUserPermissions();
        budgetsGroup.MapGetBudgetSpendings();
        budgetsGroup.MapGetOperations();
        budgetsGroup.MapAddManualSpending();
        budgetsGroup.MapAddReceiptSpending();
        budgetsGroup.MapAddReceiptFromFile();
        budgetsGroup.MapChangeReceiptProductCategory();
        budgetsGroup.MapDeleteReceiptProductCategory();
    }
}