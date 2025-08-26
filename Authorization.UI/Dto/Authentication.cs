using System.Security.Claims;
using System.Text.Json.Serialization;
using Authorization.Contracts;

namespace Authorization.UI.Dto;

public record Authentication(string AccessToken, string RefreshToken, User User, DateTime ExpiresAt)
{
    [JsonIgnore] public ClaimsPrincipal Principal => (ClaimsPrincipal) User;

    [JsonIgnore] public bool Expired => DateTime.UtcNow > ExpiresAt;
    public override string ToString()
    {
        return $"""
                Access Token: {AccessToken}
                RefreshToken: {RefreshToken}
                Login: {User?.UserName}
                """;
    }
}