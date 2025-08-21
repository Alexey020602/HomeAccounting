using Microsoft.AspNetCore.Authorization;
using Shared.Utils.Model;

namespace Budgets.Core.UserInBudgetPermissions;

public sealed class BudgetRequirementsHandler(IUserBudgetPermissionsService permissionsService)
    : AuthorizationHandler<BudgetRequirements, Guid>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        BudgetRequirements requirement, Guid resource)
    {
        if (requirement.Permission == BudgetPermissions.Undefined)
        {
            context.Fail();
        }

        if (await permissionsService.UserHasPermissionInBudget(context.User.GetUserId(), resource,
                requirement.Permission))
        {
            return;
        }

        context.Fail();
    }
}