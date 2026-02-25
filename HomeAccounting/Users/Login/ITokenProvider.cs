using System.Security.Claims;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Users.Login;

internal interface ITokenProvider
{
    ClaimsPrincipal GetPrincipal(string token);
    Data.RefreshToken CreateRefreshToken();
    AccessToken CreateToken(IReadOnlyList<Claim> claims);
}

internal record RefreshTokenResult();