using BlazorShared.Api;
using BlazorShared.Authorization;
using BlazorShared.Authorization.AuthenticationStateProvider;
using Core.Authorization;

namespace BlazorShared.Services;

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