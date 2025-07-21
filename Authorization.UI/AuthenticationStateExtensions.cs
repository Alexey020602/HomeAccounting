using Authorization.UI.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace Authorization.UI;

public static class AuthenticationStateExtensions
{
    public static AuthenticationState GetAnonymous()
    {
        return new AuthenticationState(ClaimsPrincipalExtensions.GetAnonymousPrincipal());
    }

    public static AuthenticationState GetAuthenticationState(this Authentication authentication)
    {
        return new AuthenticationState(authentication.Principal);
    }
}