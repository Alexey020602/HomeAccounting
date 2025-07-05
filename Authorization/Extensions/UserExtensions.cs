using System.Security.Claims;
using Authorization.DataBase;

namespace Authorization.Extensions;

public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? throw new Exception("Имя пользователя отстствует"))
    ];
}