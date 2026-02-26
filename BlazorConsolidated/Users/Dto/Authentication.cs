using System.Security.Claims;
using System.Text.Json.Serialization;
using ClientServerContracts.Users.GetUser;
using ClientServerShared.Users;

namespace BlazorConsolidated.Users.Dto;

public record Authentication(
    string AccessToken, 
    string RefreshToken, 
    User User, 
    DateTimeOffset ExpiresAt, 
    DateTimeOffset RefreshTokenExpiresAt)
{
    [JsonIgnore] public ClaimsPrincipal Principal => User.GetPrincipal();

    public bool AccessTokenExpired(DateTimeOffset pointTime) => ExpiresAt > pointTime;
    public bool RefreshTokenExpired(DateTimeOffset pointTime) => RefreshTokenExpiresAt > pointTime;
    public override string ToString()
    {
        return $"""
                Access Token: {AccessToken}
                RefreshToken: {RefreshToken}
                Login: {User?.UserName}
                """;
    }
}