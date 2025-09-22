using System.Security.Claims;

namespace Authorization.UI.Infrastructure;

public static class ClaimsPrincipalExtensions
{
    public static ClaimsPrincipal GetAnonymousPrincipal()
    {
        return new ClaimsPrincipal(new ClaimsIdentity());
    }
}