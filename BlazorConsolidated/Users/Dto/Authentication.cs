using System.Security.Claims;
using System.Text.Json.Serialization;
using ClientServerContracts.User.GetUser;
using ClientServerShared.Users;

namespace BlazorConsolidated.Users.Dto;

public record Authentication(string AccessToken, string RefreshToken, User User, DateTime ExpiresAt)
{
    [JsonIgnore] public ClaimsPrincipal Principal => User.GetPrincipal();

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