using BlazorShared.Api.Attributes;
using Core.Authorization;
using Refit;

namespace BlazorShared.Api;

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