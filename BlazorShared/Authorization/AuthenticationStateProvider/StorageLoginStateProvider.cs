using BlazorShared.Authorization.Dto;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;

namespace BlazorShared.Authorization.AuthenticationStateProvider;

public class StorageLoginStateProvider(IAuthenticationStorage authenticationStorage, ILogger<StorageLoginStateProvider> logger): 
    Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider,
    ILoginStateProvider
{
    private readonly IAuthenticationStorage authenticationStorage = authenticationStorage;
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var authentication = await authenticationStorage.GetAuthorizationAsync();
        logger.LogInformation("Get authentication state {authentication}", authentication);
        return authentication != null
            ? authentication.GetAuthenticationState()
            : AuthenticationStateExtensions.GetAnonymous();
    }

    public async Task Logout()
    {
        logger.LogInformation("Performing logout");
        await authenticationStorage.RemoveAuthorizationAsync();
        logger.LogInformation("Logouted from storage");
        NotifyLogout();
    }

    public async Task Login(Authentication authentication)
    {
        logger.LogInformation("Performing login");
        await authenticationStorage.SetAuthorizationAsync(authentication);
        logger.LogInformation("Login state {authentication}", authentication);
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