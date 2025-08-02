using Authorization.Contracts;
using Authorization.Core.Registration;
using MaybeResults;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorization.Core.Refresh;

public sealed class RefreshTokenHandler(IUserService userService, ITokenService tokenService)
    : IRequestHandler<RefreshTokenQuery, IMaybe<AuthorizationResponse>>
{
    public async ValueTask<IMaybe<AuthorizationResponse>> Handle(RefreshTokenQuery query,
        CancellationToken cancellationToken) =>
        (await userService.GetUserByRefreshToken(
            query.RefreshToken,
            tokenService.CreateRefreshToken,
            cancellationToken))
        .FlatMap(user =>
        {
            if (user.RefreshToken is null)
            {
                return new UserError<AuthorizationResponse>("No refresh token found");
            }

            if (user.UserName is null)
            {
                return new UserError<AuthorizationResponse>("No user name found");
            }

            return Maybe.Create(
                new AuthorizationResponse()
                {
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    UserId = user.Id,
                    Login = user.UserName,
                    AccessToken = tokenService.CreateTokenForUser(user),
                    RefreshToken = user.RefreshToken.Token,
                }
            );
        });
}