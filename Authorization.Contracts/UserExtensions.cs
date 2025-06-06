using System.Security.Claims;
using CoreUser = Authorization.Contracts.User;

namespace Authorization.Contracts;

public static class UserExtensions
{
    public static CoreUser CreateUser(this ClaimsPrincipal claimsPrincipal)
    {
        var login = claimsPrincipal.GetLogin();
        var name = claimsPrincipal.FindFirst(ClaimTypes.Name) ?? throw new Exception("Отсутствует имя пользователя");
        return new CoreUser(login, name.Value);
    }
    
    public static string GetLogin(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Отсутствует Login"); 
}