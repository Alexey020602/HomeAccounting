using System.Net;
using ClientServerContracts.Users.Login;
using ClientServerShared.Users;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Users.RefreshToken;

/// <summary>
/// Endpoint for refreshing JWT using a valid refresh token.
/// </summary>
static class RefreshTokenEndpoint
{
    /// <summary>
    /// Maps POST refresh. Exchanges valid refresh token for new JWT and refresh token.
    /// </summary>
    public static void MapRefreshToken(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapPost(
                "refresh",
                async (string refreshToken, UserManager<User> userManager, ITokenProvider tokenProvider) =>
                {
                    if (await userManager.Users.FirstOrDefaultAsync(
                            user => user.RefreshToken != null && user.RefreshToken.Token == refreshToken
                            ) is not {} user)
                    {
                        return Results.Unauthorized();
                    }

                    if (user.RefreshToken == null || user.RefreshToken.Expires < DateTimeOffset.UtcNow)
                    {
                        return Results.Unauthorized();
                    }


                    var newRefreshToken = tokenProvider.CreateRefreshToken();
                    user.AddRefreshToken(newRefreshToken);

                    await userManager.UpdateAsync(user);

                    var accessToken = tokenProvider.CreateTokenForUser(user);
                    return Results.Ok(
                        new AuthorizationResponse(
                            JwtBearerDefaults.AuthenticationScheme,
                            new(
                                user.Id.Value,
                                user.UserName ?? throw UserException.NoUserName,
                                user.FullName),
                            accessToken.Token,
                            newRefreshToken.Token,
                            accessToken.ExpiresAt
                        ));
                })
            .WithName("RefreshToken")
            .WithTags("Users")
            .WithSummary("Refresh token")
            .WithDescription("Exchanges a valid refresh token for new JWT and refresh token.")
            .Produces((int)HttpStatusCode.OK, typeof(AuthorizationResponse))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Unauthorized)
            .AllowAnonymous();
    }
}