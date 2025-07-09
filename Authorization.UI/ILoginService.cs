using Authorization.Contracts;

namespace Authorization.UI;

public interface ILoginService
{
    Task Login(LoginRequest loginRequest);
}