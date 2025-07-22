using System.Security.Claims;
using Authorization.Contracts;

namespace Authorization.Core;

public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this Core.User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? throw UserException.NoUserName),
        new Claim(ClaimsConstants.FullName, user.FullName ?? throw UserException.NoFullName)
    ];
}

