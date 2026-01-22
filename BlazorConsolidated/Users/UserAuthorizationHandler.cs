using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace BlazorConsolidated.Users;

public sealed class UserAuthorizationHandler : AuthorizationHandler<UserRequirement, Guid>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement, Guid userId)
    {
        if (
            !GetUserIdFormClaimsPrincipal(context.User, out var claimsUserId)
            || (userId != claimsUserId))
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool GetUserIdFormClaimsPrincipal(ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        if (
            claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value is not { } claimsStringId
            || !Guid.TryParse(claimsStringId, out userId)
        )
        {
            userId = Guid.Empty;
            return false;
        }

        return true;
    }
}