using Authorization.Contracts;
using Authorization.Core.Login;
using Authorization.Core.Registration;
using LightResults;

namespace Authorization.Core;

public interface IAuthenticationManager
{
    Task<Result<AuthorizationResponse>> Login(LoginQuery loginRequest);
    Task<Result<AuthorizationResponse>> Refresh(string refreshToken);
    Task<Result> Register(RegisterCommand registrationRequest);
}