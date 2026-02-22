namespace HomeAccounting.Users.Login;

public sealed record AccessToken(string Token, DateTimeOffset  ExpiresAt);