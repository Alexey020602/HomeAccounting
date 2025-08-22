using MaybeResults;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.UserInBudgetPermissions;

public sealed class UserInBudgetPermissionsHandler(IUserBudgetPermissionsService userBudgetPermissionsService):IResultQueryHandler<UserInBudgetPermissionsQuery, Contracts.UserInBudgetPermissions.UserInBudgetPermissions>
{
    public async ValueTask<IMaybe<Contracts.UserInBudgetPermissions.UserInBudgetPermissions>> Handle(UserInBudgetPermissionsQuery query, CancellationToken cancellationToken)
    {
        return from permissions in await userBudgetPermissionsService.UserInBudgetPermissions(query.UserId,
            query.BudgetId)
            select CreateFromPermissions(query.BudgetId, permissions);
    }

    private static Contracts.UserInBudgetPermissions.UserInBudgetPermissions CreateFromPermissions(Guid budgetId, BudgetPermissions permissions) =>
        new(
            budgetId,
            permissions.HasFlag(BudgetPermissions.Edit),
            permissions.HasFlag(BudgetPermissions.Delete)
        );
}