using System.Security.Claims;

namespace BlazorShared.Authorization.Dto;

public record Authentication(string AccessToken, string RefreshToken, string Login)
{
    public ClaimsPrincipal Principal => new ClaimsPrincipal(Identity);
    private ClaimsIdentity Identity => new ClaimsIdentity(Claims);

    private IReadOnlyList<Claim> Claims =>
    [
        new Claim(ClaimTypes.NameIdentifier, Login)
    ];
}
