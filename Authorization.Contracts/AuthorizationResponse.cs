namespace Authorization.Contracts;

public class AuthorizationResponse
{
    public required string Scheme { get; init; }
    public required Guid UserId { get; init; }
    public required string Login { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public override string ToString() => $"Scheme: {Scheme}\nId: {UserId} \nLogin: {Login}\n AccessToken: {AccessToken}\n RefreshToken: {RefreshToken}";
}