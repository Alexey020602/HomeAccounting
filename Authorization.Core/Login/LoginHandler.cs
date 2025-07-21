using Authorization.Contracts;
using LightResults;
using Mediator;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Authorization.Core.Login;

public sealed class LoginHandler(IUserService userService, ITokenService tokenService) : IRequestHandler<LoginQuery, Result<AuthorizationResponse>>
{
    public async ValueTask<Result<AuthorizationResponse>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var result = await userService.GetUserByRequest(
            new UserRequest(query.Login, query.Password),
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

        return Result.Success(
            new AuthorizationResponse
            {
                Scheme = JwtBearerDefaults.AuthenticationScheme,
                UserId = user.Id,
                Login = query.Login,
                AccessToken = tokenService.CreateTokenForUser(user),
                RefreshToken = user.RefreshToken.Token,
            }
        );
    }
}