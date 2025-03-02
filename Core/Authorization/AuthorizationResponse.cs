namespace Core.Authorization;

public class AuthorizationResponse
{
    public required string Scheme { get; init; }
    public required string Login { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }

    public override string ToString() => $"Scheme: {Scheme}\n Login: {Login}\n AccessToken: {AccessToken}\n RefreshToken: {RefreshToken}";
}