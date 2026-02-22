using System.Security.Claims;
using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.DependencyInjection;
using BlazorConsolidated.Users.Infrastructure;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using BlazorConsolidated.Users.Registration;
using BlazorConsolidated.Users.Registration.Validators;
using ClientServerContracts.Api.Users;
using ClientServerShared;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Users;

public static class AuthorizationModule
{
    public const string UserbyidPolicyName = "UserById";

    public static IServiceCollection AddAuthorizationModule(this IServiceCollection serviceCollection, Uri apiUri)
    {
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
            .AddSingleton<ITokenService, TokenService>()
            .AddSingleton<IAuthenticationStorage, AuthenticationStorage>()
            .Decorate<IAuthenticationStorage, TelemetryAuthenticationStorage>()
            .AddScoped<IValidator<RegistrationModel>, RegistrationModelValidator>()
            .AddSingletonAsMultipleServices<AuthenticationStateProvider, IAuthenticationStateNotifier, AuthenticationStateProviderService>()
            .AddSingleton<ILoginService, LoginService>()
            .AddSingleton<ILogoutAction, AuthenticationStorageLogoutAction>()
            .AddTransient<AuthenticationHandler>();

        serviceCollection.AddBaseRefitClient<IAuthorizationApi>(apiUri);
        serviceCollection.AddHomeAccountingRefitClient<IUsersApi>(apiUri);

        return serviceCollection;
    }
}