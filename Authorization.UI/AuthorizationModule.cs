using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Shared.Blazor;
using Shared.Utils;

namespace Authorization.UI;

public static class AuthorizationModule
{
    internal const string UserbyidPolicyName = "UserById";

    public static IServiceCollection AddAuthorizationModule(this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddAuthorizationCore(options =>
                options.AddPolicy(
                    UserbyidPolicyName,
                    policy =>
                        policy
                            .RequireAuthenticatedUser()
                            .RequireClaim(ClaimTypes.NameIdentifier)
                            .Requirements.Add(new UserRequirement())
                ))
            .AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>()
            .AddCascadingAuthenticationState()
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            .Decorate<IAuthenticationStorage, TelemetryAuthenticationStorage>()
            .AddScopedAsMultipleServices<
                ILoginService,
                AuthenticationStateProvider,
                ILogoutService,
                StorageAuthenticationService
            >();
}

public sealed class UserRequirement : IAuthorizationRequirement
{
}

public sealed class UserAuthorizationHandler : AuthorizationHandler<UserRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, UserRequirement requirement)
    {
        if (
            context.Resource is not RouteData routeData
            || GetUserIdFromContext(routeData, out var userId)
            || GetUserIdFormClaimsPrincipal(context.User, out var claimsUserId)
            || !(userId == claimsUserId)
        )
        {
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }

    private static bool GetUserIdFromContext(RouteData routeData, out Guid userId)
    {
        if (
            !routeData.RouteValues.TryGetValue("id", out var idValue)
            || Convert.ToString(idValue) is not { } idString
            || !Guid.TryParse(idString, out userId)
            )
        {
            userId = default;
            return false;
        }

        return true;
    }

    private static bool GetUserIdFormClaimsPrincipal(ClaimsPrincipal claimsPrincipal, out Guid userId)
    {
        if (
            claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value is not { } claimsStringId
            || !Guid.TryParse(claimsStringId, out userId)
            )
        {
            userId = default;
            return false;
        }

        return true;
    }
}