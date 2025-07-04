using System.Security.Claims;

namespace Shared.Model;

public static class ClaimsPrincipalExtensions
{
    public static string GetLogin(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Отсутствует Login");
}