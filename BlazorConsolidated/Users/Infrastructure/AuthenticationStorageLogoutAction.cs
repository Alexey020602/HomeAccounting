using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users.Infrastructure.Abstractions;

namespace BlazorConsolidated.Users.Infrastructure;

public sealed class AuthenticationStorageLogoutAction(
    IAuthenticationStorage storage,
    IAuthenticationStateNotifier authenticationStateNotifier
) : ILogoutAction
{
    public async Task Logout(CancellationToken cancellationToken = default)
    {
        await storage.RemoveAuthorizationAsync(cancellationToken);
        authenticationStateNotifier.NotifyAuthenticationStateChanged(Task.FromResult(AuthenticationStateExtensions.GetAnonymous()));
    }
}
