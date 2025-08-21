using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.UserInBudgetPermissions;

public record UserInBudgetPermissions(Guid BudgetId, bool CanEdit, bool CanDelete)
{
    public static UserInBudgetPermissions CreateFromPermissions(Guid budgetId, BudgetPermissions permissions) =>
        new(
            budgetId,
            permissions.HasFlag(BudgetPermissions.Edit),
            permissions.HasFlag(BudgetPermissions.Delete)
        );
}
public record UserInBudgetPermissionsQuery(Guid UserId, Guid BudgetId): IResultQuery<UserInBudgetPermissions>;