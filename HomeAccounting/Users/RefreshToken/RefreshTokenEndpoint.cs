using ClientServerContracts.User.Login;
using ClientServerShared.Users;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Users.RefreshToken;

static class RefreshTokenEndpoint
{
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
                        return Results.NotFound("User not found");
                    }

                    if (user.RefreshToken == null || user.RefreshToken.Expires < DateTime.UtcNow)
                    {
                        return Results.Unauthorized();
                    }


                    var newRefreshToken = tokenProvider.CreateRefreshToken();
                    user.AddRefreshToken(newRefreshToken);

                    await userManager.UpdateAsync(user);
                    
                    return Results.Ok(
                        new AuthorizationResponse(
                            JwtBearerDefaults.AuthenticationScheme,
                            new(
                                user.Id.Value,
                                user.UserName ?? throw UserException.NoUserName,
                                user.FullName),
                            tokenProvider.CreateTokenForUser(user),
                            newRefreshToken.Token,
                            newRefreshToken.Expires
                        ));
                });
    }
}