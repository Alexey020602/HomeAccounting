using System.Security.Claims;
using DataBase.Entities;
using CoreUser = Core.Model.User;

namespace Authorization.Extensions;

public static class UserExtensions
{
    public static IReadOnlyList<Claim> GetClaims(this User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id),
        new Claim(ClaimTypes.Name, user.UserName ?? throw new Exception("Имя пользователя отстствует"))
    ];
    
    public static CoreUser CreateUser(this ClaimsPrincipal claimsPrincipal)
    {
        var login = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("Отсутствует Login");
        var name = claimsPrincipal.FindFirst(ClaimTypes.Name) ?? throw new Exception("Отсутствует имя пользователя");
        return new CoreUser(login.Value, name.Value);
    }
}