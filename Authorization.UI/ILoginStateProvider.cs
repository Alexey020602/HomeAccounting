using Authorization.UI.Dto;

namespace Authorization.UI;

public interface ILoginStateProvider
{
    Task Logout();
    Task Login(Authentication authentication);
}