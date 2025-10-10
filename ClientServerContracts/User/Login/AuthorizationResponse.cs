using System.Text.Json.Serialization;

namespace ClientServerContracts.User.Login;

public sealed record AuthorizationResponse(
    string Scheme,
    GetUser.User User,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt)
{
    public override string ToString() => $"Scheme: {Scheme}\nId: {User}\n AccessToken: {AccessToken}\n RefreshToken: {RefreshToken}";
}

