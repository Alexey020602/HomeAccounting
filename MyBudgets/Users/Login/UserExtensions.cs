using System.Security.Claims;
using ClientServerShared.Users;
using MyBudgets.Users.Data;

namespace MyBudgets.Users.Login;

static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? throw UserException.NoUserName),
        new Claim(ClaimsConstants.FullName, user.FullName ?? throw UserException.NoFullName)
    ];
}

