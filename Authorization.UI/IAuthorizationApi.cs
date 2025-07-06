using Authorization.Contracts;
using Refit;
using Shared.Blazor.Attributes;

namespace Authorization.UI;

[Api("authorization")]
public interface IAuthorizationApi
{
    [Post("/login")]
    Task<AuthorizationResponse> Login(LoginRequest loginRequest);
    [Post("/register")]
    Task Register(RegistrationRequest registrationRequest);
    [Post("/refresh")]
    Task<AuthorizationResponse> RefreshToken(string refreshToken);
}