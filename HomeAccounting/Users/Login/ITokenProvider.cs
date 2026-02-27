using System.Security.Claims;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Users.Login;

internal interface ITokenProvider
{
    Task<TokenResult> CreateToken(User user, CancellationToken cancellationToken);
    Task<TokenResult> RefreshToken(string token, string refreshToken, CancellationToken cancellationToken);
    /// <summary>
    /// Revokes the session for the given refresh token. Idempotent.
    /// </summary>
    Task LogoutUserSession(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Revokes all sessions of the user identified by the given refresh token. Idempotent.
    /// </summary>
    Task LogoutAllUserSessions(string refreshToken, CancellationToken cancellationToken);
}

internal record TokenResult(UserId UserId, string Token, string RefreshToken, int ExpiresIn, int RefreshExpiresIn);