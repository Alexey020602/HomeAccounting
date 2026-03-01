using System.Net;
using ClientServerContracts.Users.Login;
using ClientServerShared.Users;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.Login;

/// <summary>
/// Endpoint for user authentication (login).
/// </summary>
static class LoginEndpoint
{
    /// <summary>
    /// Maps POST /login. Authenticates user and returns JWT and refresh token.
    /// </summary>
    public static void MapLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/login",
            async (LoginRequest loginRequest, UserManager<User> userManager, ITokenProvider tokenProvider, CancellationToken cancellationToken) =>
            {
                if (await userManager.FindByNameAsync(loginRequest.Login) is not { } user)
                {
                    return Results.NotFound("User not found");
                }

                if (!await userManager.CheckPasswordAsync(user, loginRequest.Password))
                {
                    return Results.BadRequest("Wrong password");
                }

                var tokenResult = await tokenProvider.CreateToken(user, cancellationToken);
                // var refreshToken = tokenProvider.CreateRefreshToken();

                // user.AddRefreshToken(refreshToken);

                // var accessToken = tokenProvider.CreateTokenForUser(user);
                return Results.Ok(
                    new TokenResponse(
                        JwtBearerDefaults.AuthenticationScheme,
                        new(
                            user.Id.Value,
                            user.UserName ?? throw UserException.NoUserName,
                            user.FullName),
                        tokenResult.Token,
                        tokenResult.RefreshToken,
                        tokenResult.ExpiresIn,
                        tokenResult.RefreshExpiresIn
                    ));
            })
            .WithName("Login")
            .WithTags("Users")
            .WithSummary("Login")
            .WithDescription("Authenticates user by login and password. Returns JWT and refresh token.")
            .Produces((int)HttpStatusCode.OK, typeof(TokenResponse))
            .AllowAnonymous();
    }
}