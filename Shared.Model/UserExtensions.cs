using System.Security.Claims;
using CoreUser = Shared.Model.User;

namespace Shared.Model;

public static class UserExtensions
{
    public static CoreUser CreateUser(this ClaimsPrincipal claimsPrincipal)
    {
        var login = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier) ?? throw new Exception("Отсутствует Login");
        var name = claimsPrincipal.FindFirst(ClaimTypes.Name) ?? throw new Exception("Отсутствует имя пользователя");
        return new CoreUser(login.Value, name.Value);
    }
}