namespace Authorization.Contracts;

public class AuthorizationResponse
{
    public AuthorizationResponse(string authorizationScheme, Guid userId, string login, string accessToken, string refreshToken)
    {
        Scheme = authorizationScheme;
        UserId = userId;
        Login = login;
        AccessToken = accessToken;
        RefreshToken = refreshToken;
        
    }
    public string Scheme { get; init; }
    public Guid UserId { get; init; }
    public string Login { get; init; }
    public string AccessToken { get; init; }
    public string RefreshToken { get; init; }
    public override string ToString() => $"Scheme: {Scheme}\nId: {UserId} \nLogin: {Login}\n AccessToken: {AccessToken}\n RefreshToken: {RefreshToken}";
}