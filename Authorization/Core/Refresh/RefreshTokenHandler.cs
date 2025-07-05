using Authorization.Contracts;
using LightResults;
using Mediator;

namespace Authorization.Core.Refresh;

public sealed class RefreshTokenHandler(IAuthenticationManager authenticationManager)
    : IRequestHandler<RefreshTokenQuery, Result<AuthorizationResponse>>
{
    public ValueTask<Result<AuthorizationResponse>> Handle(RefreshTokenQuery query, CancellationToken cancellationToken)
    {
        return new ValueTask<Result<AuthorizationResponse>>(authenticationManager.Refresh(query.RefreshToken));
    }
}