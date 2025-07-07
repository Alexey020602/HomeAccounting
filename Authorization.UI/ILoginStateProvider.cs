using Authorization.UI.Dto;
using Shared.Blazor;

namespace Authorization.UI;

public interface ILoginStateProvider: ILogoutService
{
    Task Login(Authentication authentication);
}