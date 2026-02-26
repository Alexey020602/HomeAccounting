namespace ClientServerContracts.Users.Login;

public sealed record TokenResponse(
    string TokenType,
    GetUser.User User,
    string AccessToken,
    string RefreshToken,
    int ExpiresIn,
    int RefreshExpiresIn)
{
    public override string ToString() => $"TokenType: {TokenType}\nId: {User}\n AccessToken: {AccessToken}\n RefreshToken: {RefreshToken}";
}

