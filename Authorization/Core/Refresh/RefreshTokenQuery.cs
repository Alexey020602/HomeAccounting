using Authorization.Contracts;
using LightResults;
using Mediator;

namespace Authorization.Core.Refresh;

public record RefreshTokenQuery(string RefreshToken) : IRequest<Result<AuthorizationResponse>>;