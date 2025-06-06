using System.Security.Claims;
using Checks.DataBase.Entities;
using CoreUser = Authorization.Contracts.User;

namespace Authorization.Extensions;

public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? throw new Exception("Имя пользователя отстствует"))
    ];
}