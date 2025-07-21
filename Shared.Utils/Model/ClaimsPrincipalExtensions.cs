using System.Security.Claims;

namespace Shared.Utils.Model;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal) => Guid.Parse(claimsPrincipal.GetStringUserId());
        
        private static string GetStringUserId(this ClaimsPrincipal claimsPrincipal) => 
            claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Отсутствует Login");
    
}