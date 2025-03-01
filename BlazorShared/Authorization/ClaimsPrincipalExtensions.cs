using System.Security.Claims;

namespace BlazorShared.Authorization;

public static class ClaimsPrincipalExtensions
{
    public static ClaimsPrincipal GetAnonymousPrincipal() => new ClaimsPrincipal(new ClaimsIdentity());
}