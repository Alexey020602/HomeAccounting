using MaybeResults;

namespace Budgets.Core.UserInBudgetPermissions;

public interface IUserBudgetPermissionsService
{
    public Task<IMaybe<BudgetPermissions>> UserInBudgetPermissions(Guid userId, Guid budgetId);
    
}

public static class UserInBudgetPermissionsServiceExtensions
{
    public static async Task<bool> UserHasPermissionInBudget(
        this IUserBudgetPermissionsService userBudgetPermissionsService,
        Guid userId,
        Guid budgetId,
        BudgetPermissions permission) =>
        await userBudgetPermissionsService.UserInBudgetPermissions(userId, budgetId) switch
        {
            Some<BudgetPermissions> some => some.Value.HasFlag(permission),
            _ => false
        };
}