using BlazorConsolidated.Common.Attributes;
using ClientServerContracts.User.Login;
using ClientServerContracts.User.Register;
using Refit;

namespace BlazorConsolidated.Users.Infrastructure;

[Api]
public interface IAuthorizationApi
{
    [Get("/login/exist")]
    Task<bool> CheckLoginExist(string login);

    [Post("/login")]
    Task<AuthorizationResponse> Login(LoginRequest loginRequest);

    [Post("/register")]
    Task Register(RegistrationRequest registrationRequest);

    [Post("/refresh")]
    Task<AuthorizationResponse> RefreshToken(string refreshToken);
}