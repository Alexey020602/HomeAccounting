using Authorization.Contracts;
using Authorization.Core.Login;
using Authorization.Core.Registration;
using Authorization.Extensions;
using LightResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorization.Core;

public sealed class RefreshTokenError : Error;

public sealed class AuthenticationManager(
    IUserService userService,
    ITokenService tokenService
) : IAuthenticationManager
{
    public async Task<Result<AuthorizationResponse>> Login(LoginQuery loginRequest)
    {
        var result = await userService.GetUserByRequest(
            new UserRequest(loginRequest.Login, loginRequest.Password),
            tokenService.CreateRefreshToken
        );
        if (result.IsFailure(out var error, out var user))
        {
            return Result.Failure<AuthorizationResponse>(error);
        }

        if (user.RefreshToken is null)
        {
            return Result.Failure<AuthorizationResponse>("No refresh token found");
        }

        return Result.Success(
            new AuthorizationResponse
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = loginRequest.Login,
                AccessToken = CreateAccessToken(user),
                RefreshToken = user.RefreshToken.Token,
            }
        );
    }

    public async Task<Result<AuthorizationResponse>> Refresh(string refreshToken)
    {
        var result = await userService.GetUserByRefreshToken(
            refreshToken,
            tokenService.CreateRefreshToken
        );

        if (result.IsFailure(out var error, out var user))
        {
            return Result.Failure<AuthorizationResponse>(error);
        }

        if (user.RefreshToken is null)
        {
            return Result.Failure<AuthorizationResponse>("No refresh token found");
        }

        return Result.Success(
            new AuthorizationResponse()
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = user.Id,
                AccessToken = CreateAccessToken(user),
                RefreshToken = user.RefreshToken.Token,
            }
        );
    }

    public Task<Result> Register(RegisterCommand registrationRequest)
    {
        return userService.AddUser(
            new UnregisteredUser()
            {
                Login = registrationRequest.Login,
                UserName = registrationRequest.Username,
            },
            registrationRequest.Password);
    }

    private string CreateAccessToken(User user)
    {
        return tokenService.CreateToken(user.GetClaims());
    }
}