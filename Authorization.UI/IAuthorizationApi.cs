using Authorization.Contracts;
using Refit;
using Shared.Blazor.Attributes;

namespace Authorization.UI;

[Api("authorization")]
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