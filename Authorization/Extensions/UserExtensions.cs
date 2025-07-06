using System.Security.Claims;
using Authorization.DataBase;

namespace Authorization.Extensions;

public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this Core.User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Login),
        new Claim(ClaimTypes.Name, user.Username /*?? throw new Exception("Имя пользователя отстствует")*/)
    ];
}