using ClientServerContracts.User.Login;

namespace BlazorConsolidated.Users.Infrastructure;

public interface ILoginService
{
    Task Login(LoginRequest loginRequest);
}