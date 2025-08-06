using Authorization.Contracts;
using Authorization.Core.Registration;
using MaybeResults;
using Mediator;

namespace Authorization.Core.Login;

public sealed class LoginHandler(IUserService userService, ITokenService tokenService)
    : IRequestHandler<LoginQuery, IMaybe<LoginResponse>>
{
    public async ValueTask<IMaybe<LoginResponse>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        var result = await userService.GetUserByRequest(
            new UserRequest(query.Login, query.Password),
            tokenService.CreateRefreshToken, cancellationToken);

        return result.SelectMany(
            secondMaybe: user => user.RefreshToken is null
                ? new UserError<LoginResponse>("No refresh token found")
                : Maybe.Create(new LoginResponse(
                    user.Id,
                    query.Login,
                    tokenService.CreateTokenForUser(user),
                    user.RefreshToken.Token
                )),
            resultSelector: (_, response) => response
        );
        // if (result.IsFailure(out var error, out var user))
        // {
        //     return Result.Failure<AuthorizationResponse>(error);
        // }
        //
        // if (user.RefreshToken is null)
        // {
        //     return Result.Failure<AuthorizationResponse>("No refresh token found");
        // }
        //
        // return Result.Success(
        //     new AuthorizationResponse
        //     {
        //         Scheme = JwtBearerDefaults.AuthenticationScheme,
        //         UserId = user.Id,
        //         Login = query.Login,
        //         AccessToken = tokenService.CreateTokenForUser(user),
        //         RefreshToken = user.RefreshToken.Token,
        //     }
        // );
    }
}