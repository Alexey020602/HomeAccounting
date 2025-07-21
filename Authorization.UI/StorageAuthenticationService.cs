using Authorization.Contracts;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Blazor;

namespace Authorization.UI;

public sealed class StorageAuthenticationService(
    IAuthorizationApi authorizationApi,
    IAuthenticationStorage storage
)
    : AuthenticationStateProvider, ILoginService, ILogoutService
{
    public async Task Login(LoginRequest loginRequest)
    {
        var authentication = (await authorizationApi.Login(loginRequest)).ConvertToAuthentication();
        await storage.SetAuthorizationAsync(
            authentication
        );
        NotifyAuthenticationStateChanged(Task.FromResult(authentication.GetAuthenticationState()));
    }

    public async Task Logout()
    {
        await storage.RemoveAuthorizationAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(AuthenticationStateExtensions.GetAnonymous()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return (await storage.GetAuthorizationAsync())?.GetAuthenticationState() ??
               AuthenticationStateExtensions.GetAnonymous();
    }
}