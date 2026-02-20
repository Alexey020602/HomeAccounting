using BlazorConsolidated.Users.Infrastructure.Abstractions;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorConsolidated.Users.Infrastructure;

public sealed class AuthenticationStateProviderService(
    IAuthenticationStorage storage
) : AuthenticationStateProvider, IAuthenticationStateNotifier
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await storage.GetAuthorizationAsync() is not { } authentication || authentication.Expired)
        {
            return AuthenticationStateExtensions.GetAnonymous();
        }

        return authentication.GetAuthenticationState();
    }

    void IAuthenticationStateNotifier.NotifyAuthenticationStateChanged(Task<AuthenticationState> task)
    {
        NotifyAuthenticationStateChanged(task);
    }
}
