using System.Security.Claims;

namespace ClientServerShared.Users;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal) => Guid.Parse(claimsPrincipal.GetStringUserId());
        
    private static string GetStringUserId(this ClaimsPrincipal claimsPrincipal) => 
        claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new Exception("Отсутствует Login");
    public static string? GetUserName(this ClaimsPrincipal principal) => principal.FindFirst(ClaimTypes.Name)?.Value;
    public static string? GetFullName(this ClaimsPrincipal principal) => principal.FindFirst(ClaimsConstants.FullName)?.Value;

    
}