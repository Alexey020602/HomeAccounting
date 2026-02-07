using System.Security.Claims;
using System.Text.Json.Serialization;
using ClientServerContracts.Users.GetUser;
using ClientServerShared.Users;

namespace BlazorConsolidated.Users.Dto;

public record Authentication(string AccessToken, string RefreshToken, User User, DateTimeOffset ExpiresAt)
{
    [JsonIgnore] public ClaimsPrincipal Principal => User.GetPrincipal();

    [JsonIgnore] public bool Expired => DateTimeOffset.UtcNow > ExpiresAt;
    public override string ToString()
    {
        return $"""
                Access Token: {AccessToken}
                RefreshToken: {RefreshToken}
                Login: {User?.UserName}
                """;
    }
}