using System.Net;
using ClientServerContracts.Users.Login;
using ClientServerContracts.Users.Refresh;
using ClientServerShared.Users;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HomeAccounting.Users.Refresh;

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
                async (RefreshTokenRequest request, UserManager<User> userManager, ITokenProvider tokenProvider, CancellationToken cancellationToken) =>
                {
                    TokenResult tokenResult;
                    try
                    {
                        tokenResult = await tokenProvider.RefreshToken(request.Token, request.RefreshToken, cancellationToken);
                    }
                    catch (SecurityTokenException)
                    {
                        return Results.Unauthorized();
                    }
                    
                    var user = await userManager.FindByIdAsync(tokenResult.UserId);

                    if (user is null)
                    {
                        return Results.Unauthorized();
                    }
                    
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
            .WithName("RefreshToken")
            .WithTags("Users")
            .WithSummary("Refresh token")
            .WithDescription("Exchanges a valid refresh token for new JWT and refresh token.")
            .Produces((int)HttpStatusCode.OK, typeof(TokenResponse))
            .ProducesProblem((int)HttpStatusCode.BadRequest)
            .ProducesProblem((int)HttpStatusCode.Unauthorized)
            .AllowAnonymous();
    }
}