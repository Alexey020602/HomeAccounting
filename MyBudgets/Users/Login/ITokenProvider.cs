using System.Security.Claims;
using MyBudgets.Users.Data;

namespace MyBudgets.Users.Login;

public interface ITokenProvider
{
    ClaimsPrincipal GetPrincipal(string token);
    RefreshToken CreateRefreshToken();
    string CreateToken(IReadOnlyList<Claim> claims);
}