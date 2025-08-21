using Budgets.Core.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public sealed class UserBudgetPermissionsService(BudgetsContext budgetsContext): IUserBudgetPermissionsService
{
    public async Task<bool> UserHasPermissionInBudget(Guid userId, Guid budgetId, BudgetPermissions permission)
    {
        if (await budgetsContext.BudgetUsers
                .Include(user => user.BudgetRole)
                .SingleOrDefaultAsync(user => user.UserId == userId && user.BudgetId == budgetId) is not { } budgetUser)
        {
            return false;
        }

        if (budgetUser.BudgetRole is null)
        {
            throw new InvalidOperationException("User has no budget role");
        }
        return budgetUser.BudgetRole.Permissions.HasFlag(permission);
    }
}