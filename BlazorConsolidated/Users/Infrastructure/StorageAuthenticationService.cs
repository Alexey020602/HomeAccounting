using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Api.Users;
using ClientServerContracts.Users.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorConsolidated.Users.Infrastructure;
public sealed class StorageAuthenticationService(
    IAuthorizationApi authorizationApi,
    IAuthenticationStorage storage
)
    : AuthenticationStateProvider, ILoginService, ILogoutAction
{
    public async Task Login(LoginRequest loginRequest)
    {
        var authentication = (await authorizationApi.Login(loginRequest)).ConvertToAuthentication();
        await storage.SetAuthorizationAsync(
            authentication
        );
        NotifyAuthenticationStateChanged(Task.FromResult(authentication.GetAuthenticationState()));
    }

    public async Task Logout(CancellationToken cancellationToken = default)
    {
        await storage.RemoveAuthorizationAsync(cancellationToken);
        NotifyAuthenticationStateChanged(Task.FromResult(AuthenticationStateExtensions.GetAnonymous()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await storage.GetAuthorizationAsync() is not { } authentication || authentication.Expired)
        {
            return AuthenticationStateExtensions.GetAnonymous();
        }

        return authentication.GetAuthenticationState();
    }
}