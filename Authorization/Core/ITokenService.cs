using System.Security.Claims;
using Authorization.DataBase;

namespace Authorization.Core;

public interface ITokenService
{
    ClaimsPrincipal GetPrincipal(string token);
    RefreshToken CreateRefreshToken();
    string CreateToken(IReadOnlyList<Claim> claims);
}