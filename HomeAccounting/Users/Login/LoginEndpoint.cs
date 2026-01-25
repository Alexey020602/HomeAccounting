using System.Net;
using ClientServerContracts.User.Login;
using ClientServerShared.Users;
using HomeAccounting.Users.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace HomeAccounting.Users.Login;

static class LoginEndpoint
{
    public static void MapLogin(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/login",
            async (LoginRequest loginRequest, UserManager<User> userManager, ITokenProvider tokenProvider) =>
            {
                if (await userManager.FindByNameAsync(loginRequest.Login) is not { } user)
                {
                    return Results.NotFound("User not found");
                }

                if (!await userManager.CheckPasswordAsync(user, loginRequest.Password))
                {
                    return Results.BadRequest("Wrong password");
                }

                var refreshToken = tokenProvider.CreateRefreshToken();

                user.AddRefreshToken(refreshToken);

                return Results.Ok(
                    new AuthorizationResponse(
                        JwtBearerDefaults.AuthenticationScheme,
                        new(
                            user.Id.Value,
                            user.UserName ?? throw UserException.NoUserName,
                            user.FullName),
                        tokenProvider.CreateTokenForUser(user),
                        refreshToken.Token,
                        refreshToken.Expires
                    ));
            })
            .Produces((int) HttpStatusCode.OK, typeof(AuthorizationResponse))
            .AllowAnonymous();
    }
}