using Budgets.Core.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using MaybeResults;
using Microsoft.EntityFrameworkCore;

namespace Budgets.DataBase;

public sealed class UserBudgetPermissionsService(BudgetsContext budgetsContext): IUserBudgetPermissionsService
{
    public async Task<IMaybe<BudgetPermissions>> UserInBudgetPermissions(Guid userId, Guid budgetId)
    {
        if (await budgetsContext.BudgetUsers
                .Include(user => user.BudgetRole)
                .SingleOrDefaultAsync(user => user.UserId == userId && user.BudgetId == budgetId) is not { } budgetUser)
        {
            return new UserInBudgetNotFound<BudgetPermissions>("User for budget not found");
        }

        if (budgetUser.BudgetRole is null)
        {
            throw new InvalidOperationException("User has no budget role");
        }
        return Maybe.Create(budgetUser.BudgetRole.Permissions);
    }
}