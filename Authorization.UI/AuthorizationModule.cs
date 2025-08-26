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
    public const string UserbyidPolicyName = "UserById";

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