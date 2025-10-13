using System.Security.Claims;
using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users.Infrastructure;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using BlazorConsolidated.Users.Registration;
using BlazorConsolidated.Users.Registration.Validators;
using ClientServerShared;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Users;

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
            .AddScoped<ITokenService, TokenService>()
            .AddScoped<IAuthenticationStorage, AuthenticationStorage>()
            .Decorate<IAuthenticationStorage, TelemetryAuthenticationStorage>()
            .AddScoped<IValidator<RegistrationModel>, RegistrationModelValidator>()
            .AddScopedAsMultipleServices<
                ILoginService,
                AuthenticationStateProvider,
                ILogoutAction,
                StorageAuthenticationService
            >();
}