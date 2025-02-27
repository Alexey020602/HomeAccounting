using System.Security.Claims;
using DataBase.Entities;

namespace Authorization;

public interface ITokenService
{
    ClaimsPrincipal GetPrincipal(string token);
    RefreshToken CreateRefreshToken();
    string CreateToken(IReadOnlyList<Claim> claims);
}