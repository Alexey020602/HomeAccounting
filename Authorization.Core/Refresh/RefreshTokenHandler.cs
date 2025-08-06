using Authorization.Contracts;
using Authorization.Core.Login;
using Authorization.Core.Registration;
using MaybeResults;
using Mediator;

namespace Authorization.Core.Refresh;

public sealed class RefreshTokenHandler(IUserService userService, ITokenService tokenService)
    : IRequestHandler<RefreshTokenQuery, IMaybe<LoginResponse>>
{
    public async ValueTask<IMaybe<LoginResponse>> Handle(RefreshTokenQuery query,
        CancellationToken cancellationToken) =>
        (await userService.GetUserByRefreshToken(
            query.RefreshToken,
            tokenService.CreateRefreshToken,
            cancellationToken))
        .FlatMap(user =>
        {
            if (user.RefreshToken is null)
            {
                return new UserError<LoginResponse>("No refresh token found");
            }

            if (user.UserName is null)
            {
                return new UserError<LoginResponse>("No user name found");
            }

            return Maybe.Create(
                new LoginResponse(
                    user.Id,
                    user.UserName,
                    tokenService.CreateTokenForUser(user),
                    user.RefreshToken.Token
                )
            );
        });
}