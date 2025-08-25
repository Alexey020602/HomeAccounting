using Microsoft.AspNetCore.Authorization;
using Shared.Utils.Model;

namespace Budgets.Core.UserInBudgetPermissions;

public sealed class BudgetRequirementsAuthorizationHandler(IUserBudgetPermissionsService permissionsService)
    : AuthorizationHandler<BudgetRequirements, Guid>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        BudgetRequirements requirement, Guid resource)
    {
        if (requirement.Permission == BudgetPermissions.Undefined)
        {
            context.Fail(new AuthorizationFailureReason(this, "User permissions not set"));
        }

        if (await permissionsService.UserHasPermissionInBudget(context.User.GetUserId(), resource,
                requirement.Permission))
        {
            context.Succeed(requirement);
            return;
        }

        context.Fail(new  AuthorizationFailureReason(this, "User has no required permission"));
    }
}