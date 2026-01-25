using System.Security.Claims;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Users.Login;

public interface ITokenProvider
{
    ClaimsPrincipal GetPrincipal(string token);
    Data.RefreshToken CreateRefreshToken();
    string CreateToken(IReadOnlyList<Claim> claims);
}