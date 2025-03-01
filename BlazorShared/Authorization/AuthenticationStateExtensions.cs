using System.Security.Claims;
using BlazorShared.Authorization.Dto;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorShared.Authorization;

public static class AuthenticationStateExtensions
{
    public static AuthenticationState GetAnonymous() => new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    public static AuthenticationState GetAuthenticationState(this Authentication authentication) => 
        new AuthenticationState(authentication.Principal);
}