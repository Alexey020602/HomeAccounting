namespace ClientServerContracts.Users.Refresh;

public sealed record RefreshTokenRequest(string Token, string RefreshToken);