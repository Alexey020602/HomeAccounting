using System.Security.Claims;
using Authorization.UI.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace Authorization.UI;

public static class AuthenticationStateExtensions
{
    public static AuthenticationState GetAnonymous() => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    public static AuthenticationState GetAuthenticationState(this Authentication authentication) => 
        new AuthenticationState(authentication.Principal);
}