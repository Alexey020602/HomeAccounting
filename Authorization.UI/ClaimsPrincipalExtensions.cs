using System.Security.Claims;

namespace Authorization.UI;

public static class ClaimsPrincipalExtensions
{
    public static ClaimsPrincipal GetAnonymousPrincipal() => new ClaimsPrincipal(new ClaimsIdentity());
}