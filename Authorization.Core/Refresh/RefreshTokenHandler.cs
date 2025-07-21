using Authorization.Contracts;
using LightResults;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorization.Core.Refresh;

public sealed class RefreshTokenHandler(IUserService userService, ITokenService tokenService)
    : IRequestHandler<RefreshTokenQuery, Result<AuthorizationResponse>>
{
    public async ValueTask<Result<AuthorizationResponse>> Handle(RefreshTokenQuery query, CancellationToken cancellationToken)
    {
        var result = await userService.GetUserByRefreshToken(
            query.RefreshToken,
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

        if (user.UserName is null)
        {
            return Result.Failure<AuthorizationResponse>("No user name found");
        }

        return Result.Success(
            new AuthorizationResponse()
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                UserId = user.Id,
                Login = user.UserName,
                AccessToken = tokenService.CreateTokenForUser(user),
                RefreshToken = user.RefreshToken.Token,
            }
        );
    }
}