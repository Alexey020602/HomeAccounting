using System.Security.Claims;

namespace Authorization;

public interface ITokenService
{
    ClaimsPrincipal GetPrincipal(string token);
    string CreateRefreshToken();
    string CreateToken(IReadOnlyList<Claim> claims);
}