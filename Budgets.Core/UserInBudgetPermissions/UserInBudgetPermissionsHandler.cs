using MaybeResults;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.UserInBudgetPermissions;

public sealed class UserInBudgetPermissionsHandler(IUserBudgetPermissionsService userBudgetPermissionsService):IResultQueryHandler<UserInBudgetPermissionsQuery, UserInBudgetPermissions>
{
    public async ValueTask<IMaybe<UserInBudgetPermissions>> Handle(UserInBudgetPermissionsQuery query, CancellationToken cancellationToken)
    {
        return from permissions in await userBudgetPermissionsService.UserInBudgetPermissions(query.UserId,
            query.BudgetId)
            select UserInBudgetPermissions.CreateFromPermissions(query.BudgetId, permissions);
    }
}