using System.Security.Claims;
using System.Text.Json.Serialization;
using Authorization.Contracts;

namespace Authorization.UI.Dto;

public record Authentication(string AccessToken, string RefreshToken, User User)
{
    [JsonIgnore] public ClaimsPrincipal Principal => new(Identity);

    [JsonIgnore] private ClaimsIdentity Identity => new(Claims, "jwtAuthType");

    [JsonIgnore]
    private IReadOnlyList<Claim> Claims =>
    [
        new(ClaimTypes.NameIdentifier, User.Id.ToString()),
        new(ClaimTypes.Name, User.UserName)
    ];

    public override string ToString()
    {
        return $"""
                Access Token: {AccessToken}
                RefreshToken: {RefreshToken}
                Login: {User?.UserName}
                """;
    }
}