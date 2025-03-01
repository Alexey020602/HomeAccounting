using BlazorShared.Authorization.Dto;

namespace BlazorShared.Authorization.AuthenticationStateProvider;

public interface ILoginStateProvider
{
    Task Logout();
    Task Login(Authentication authentication);
}