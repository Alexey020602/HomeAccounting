using Authorization.Contracts;
using Authorization.Core.Login;
using Authorization.Core.Registration;
using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Authorization.Core.Refresh;

public sealed class RefreshTokenHandler(IUserService userService, ITokenService tokenService)
    : IResultRequestHandler<RefreshTokenQuery, LoginResponse>
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
                    user,
                    tokenService.CreateTokenForUser(user),
                    user.RefreshToken.Token,
                    user.RefreshToken.Expires
                )
            );
        });
}