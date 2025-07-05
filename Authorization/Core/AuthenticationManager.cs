using Authorization.Contracts;
using Authorization.Core.Login;
using Authorization.Core.Registration;
using Authorization.DataBase;
using Authorization.Extensions;
using LightResults;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization.Core;

public sealed class RefreshTokenError : Error;

public sealed class AuthenticationManager(
    UserManager<User> userManager,
    ITokenService tokenService
) : IAuthenticationManager
{
    public async Task<Result<AuthorizationResponse>> Login(LoginQuery loginRequest)
    {
        var user = await userManager.FindByIdAsync(loginRequest.Login);
        // var user = await userService.GetUserByLogin(loginRequest.Login);

        if (user is null) return Result.Failure<AuthorizationResponse>("User not found");

        if (!await userManager.CheckPasswordAsync(user, loginRequest.Password))
            return Result.Failure<AuthorizationResponse>("Wrong Password");

        var userRefreshToken = await CreateRefreshToken(user);
        
        return Result.Success(
            new AuthorizationResponse
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = loginRequest.Login,
                AccessToken = CreateAccessToken(user),
                RefreshToken = userRefreshToken.Token,
            }
        );
    }
    public async Task<Result<AuthorizationResponse>> Refresh(string refreshToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(user =>
            user.RefreshTokenToken != null && user.RefreshTokenToken.Token == refreshToken);
        if (user is null)
        {
            return Result.Failure<AuthorizationResponse>("User Not Found");
        }

        if (user.RefreshTokenToken is null || user.RefreshTokenToken.Expires < DateTime.UtcNow)
        {
            return Result.Failure<AuthorizationResponse>(new RefreshTokenError());
        }

        var userRefreshToken = await CreateRefreshToken(user);

        return Result.Success(
            new AuthorizationResponse()
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                Login = user.Id,
                AccessToken = CreateAccessToken(user),
                RefreshToken = userRefreshToken.Token,
            }
        );
    }

    public async Task<Result> Register(RegisterCommand registrationRequest)
    {
        var existingUser = await userManager.FindByIdAsync(registrationRequest.Login);
        if (existingUser is not null) return Result.Failure("User already exists");

        var creationResult = await userManager.CreateAsync(
            new User
            {
                Id = registrationRequest.Login,
                UserName = registrationRequest.Username,
            },
            registrationRequest.Password
        );

        if (creationResult.Succeeded)
        {
            return Result.Success();
        }

        return Result.Failure(ConvertToErrors(creationResult));
    }

    private static IEnumerable<IError> ConvertToErrors(IdentityResult identityResult) =>
        identityResult.Errors.Select(ConvertToError);
    private static IError ConvertToError(IdentityError identityError) => new Error(identityError.Description);
    private string CreateAccessToken(User user)
    {
        return tokenService.CreateToken(user.GetClaims());
    }
    private async Task<RefreshToken> CreateRefreshToken(User user)
    {
        var refreshToken = tokenService.CreateRefreshToken();
        user.RefreshTokenToken = refreshToken;

        await userManager.UpdateAsync(user);
        return refreshToken;
    }
}