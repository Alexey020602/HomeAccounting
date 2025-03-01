using BlazorShared.Authorization.AuthenticationStateProvider;
using BlazorShared.Authorization.Dto;
using BlazorShared.Utils;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorShared.Authorization;

public class StorageAuthenticationStateProvider(IAuthenticationStorage authenticationStorage) : LoginAuthenticationStateProvider
{
    private readonly IAuthenticationStorage authenticationStorage = authenticationStorage;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync();
        return authentication != null
            ? authentication.GetAuthenticationState()
            : AuthenticationStateExtensions.GetAnonymous();
    }

    public override async Task Logout()
    {
        await authenticationStorage.RemoveAuthorizationAsync();
        NotifyLogout();
    }

    public override async Task Login(Authentication authentication)
    {
        await authenticationStorage.SetAuthorizationAsync(authentication);
        NotifyLogin(authentication);
    }

    private void NotifyLogout()
    {
        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(ClaimsPrincipalExtensions.GetAnonymousPrincipal()))
        );
    }

    private void NotifyLogin(Authentication authentication)
    {
        NotifyAuthenticationStateChanged(Task.FromResult(authentication.GetAuthenticationState()));
    }
}