using System.Security.Claims;
using DataBase.Entities;

namespace Authorization;

public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? throw new Exception("Имя пользователя отстствует"))
    ];
}