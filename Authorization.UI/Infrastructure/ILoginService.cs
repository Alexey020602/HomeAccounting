using Authorization.Contracts;

namespace Authorization.UI.Infrastructure;

public interface ILoginService
{
    Task Login(LoginRequest loginRequest);
}