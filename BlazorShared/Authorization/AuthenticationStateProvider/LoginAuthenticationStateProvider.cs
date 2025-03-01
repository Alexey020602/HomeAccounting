using BlazorShared.Authorization.Dto;

namespace BlazorShared.Authorization.AuthenticationStateProvider;

public abstract class LoginAuthenticationStateProvider: Microsoft.AspNetCore.Components.Authorization.AuthenticationStateProvider
{
    public abstract Task Logout();
    public abstract Task Login(Authentication authentication);
}