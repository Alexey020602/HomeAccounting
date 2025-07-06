using Authorization.Contracts;
using Authorization.UI;
using Authorization.UI.AuthenticationStateProvider;
using BlazorConsolidated.Api;
using Shared.Blazor;

namespace BlazorConsolidated.Services;

public interface ILoginService
{
    Task Login(LoginRequest loginRequest);
    Task Logout();
}

public class LoginService(IAuthorizationApi authorizationApi, ILoginStateProvider loginStateProvider): ILoginService
{
    public async Task Login(LoginRequest loginRequest) =>
        await loginStateProvider.Login((await authorizationApi.Login(loginRequest)).ConvertToAuthentication());
    

    public Task Logout() => loginStateProvider.Logout();
}