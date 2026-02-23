using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Api.Users;
using ClientServerContracts.Users.Login;

namespace BlazorConsolidated.Users.Infrastructure;

public sealed class LoginService(
    IAuthorizationApi authorizationApi,
    IAuthenticationStorage storage,
    IAuthenticationStateNotifier authenticationStateNotifier
) : ILoginService
{
    public async Task Login(LoginRequest loginRequest, CancellationToken cancellationToken = default)
    {
        var authentication = (await authorizationApi.Login(loginRequest)).ConvertToAuthentication();
        await storage.SetAuthorizationAsync(authentication, cancellationToken);
        authenticationStateNotifier.NotifyAuthenticationStateChanged(Task.FromResult(authentication.GetAuthenticationState()));
    }
}
