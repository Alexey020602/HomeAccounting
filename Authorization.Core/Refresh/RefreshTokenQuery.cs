using Authorization.Contracts;
using Authorization.Core.Login;

namespace Authorization.Core.Refresh;

public record RefreshTokenQuery(string RefreshToken) : IMaybeRequest<AuthorizationResponse>;