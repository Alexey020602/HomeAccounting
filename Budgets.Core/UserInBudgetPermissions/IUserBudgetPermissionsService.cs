namespace Budgets.Core.UserInBudgetPermissions;

public interface IUserBudgetPermissionsService
{
    public Task<bool> UserHasPermissionInBudget(Guid userId, Guid budgetId, BudgetPermissions permission);
}