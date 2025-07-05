using Authorization.Contracts;
using LightResults;
using Mediator;

namespace Authorization.Core.Login;

public sealed class LoginHandler(IAuthenticationManager authenticationManager) : IRequestHandler<LoginQuery, Result<AuthorizationResponse>>
{
    public ValueTask<Result<AuthorizationResponse>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        return new ValueTask<Result<AuthorizationResponse>>(authenticationManager.Login(query));
    }
}