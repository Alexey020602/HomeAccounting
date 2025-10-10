using System.Security.Claims;

namespace BlazorConsolidated.Users.Infrastructure;

public static class ClaimsPrincipalExtensions
{
    public static ClaimsPrincipal GetAnonymousPrincipal()
    {
        return new ClaimsPrincipal(new ClaimsIdentity());
    }
}