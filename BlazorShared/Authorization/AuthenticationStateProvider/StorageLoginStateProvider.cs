using BlazorShared.Authorization.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorShared.Authorization.AuthenticationStateProvider;

public class StorageLoginStateProvider(IAuthenticationStorage authenticationStorage): 
    Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider,
    ILoginStateProvider
{
    private readonly IAuthenticationStorage authenticationStorage = authenticationStorage;

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync();
        return authentication != null
            ? authentication.GetAuthenticationState()
            : AuthenticationStateExtensions.GetAnonymous();
    }

    public async Task Logout()
    {
        await authenticationStorage.RemoveAuthorizationAsync();
        NotifyLogout();
    }

    public async Task Login(Authentication authentication)
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