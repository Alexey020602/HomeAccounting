using System.Security.Claims;
using Shared.Model;
using CoreUser = Authorization.Contracts.User;

namespace Authorization;

public static class UserExtensions
{
    public static CoreUser CreateUser(this ClaimsPrincipal claimsPrincipal)
    {
        var login = claimsPrincipal.GetLogin();
        var name = claimsPrincipal.FindFirst(ClaimTypes.Name) ?? throw new Exception("Отсутствует имя пользователя");
        return new CoreUser(login, name.Value);
    }
}