using System.Security.Claims;

namespace BlazorShared.Authorization.Dto;

public record Authentication(string AccessToken, string RefreshToken, string Login)
{
    public ClaimsPrincipal Principal => new ClaimsPrincipal(Identity);
    private ClaimsIdentity Identity => new ClaimsIdentity(Claims, "jwtAuthType");

    private IReadOnlyList<Claim> Claims =>
    [
        new Claim(ClaimTypes.NameIdentifier, Login)
    ];

    public override string ToString() => $"""
                                         Access Token: {AccessToken}
                                         RefreshToken: {RefreshToken}
                                         Login: {Login}
                                         """;
}
