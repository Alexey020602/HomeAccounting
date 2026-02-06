using ClientServerContracts.Users.Login;

namespace BlazorConsolidated.Users.Infrastructure.Abstractions;

public interface ILoginService
{
    Task Login(LoginRequest loginRequest);
}