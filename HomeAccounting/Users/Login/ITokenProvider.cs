using System.Security.Claims;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Users.Login;

internal interface ITokenProvider
{
    Task<TokenResult> CreateToken(User user, CancellationToken cancellationToken);
    Task<TokenResult> RefreshToken(string token, string refreshToken, CancellationToken cancellationToken);
}

internal record TokenResult(UserId UserId, string Token, string RefreshToken, int ExpiresIn, int RefreshExpiresIn);