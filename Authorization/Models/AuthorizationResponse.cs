namespace Authorization.Models;

public class AuthorizationResponse
{
    public required string Scheme { get; init; }
    public required string Login { get; init; }
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
}