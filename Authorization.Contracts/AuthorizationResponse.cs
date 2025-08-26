using System.Text.Json.Serialization;

namespace Authorization.Contracts;

[method: JsonConstructor]
public sealed record AuthorizationResponse(
    string Scheme,
    User User,
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt)
{
    public override string ToString() => $"Scheme: {Scheme}\nId: {User}\n AccessToken: {AccessToken}\n RefreshToken: {RefreshToken}";
}

