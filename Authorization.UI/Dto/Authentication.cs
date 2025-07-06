using System.Security.Claims;
using System.Text.Json.Serialization;

namespace Authorization.UI.Dto;

public record Authentication(string AccessToken, string RefreshToken, string Login)
{
    [JsonIgnore]
    public ClaimsPrincipal Principal => new ClaimsPrincipal(Identity);
    [JsonIgnore]
    private ClaimsIdentity Identity => new ClaimsIdentity(Claims, "jwtAuthType");
    [JsonIgnore]
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
