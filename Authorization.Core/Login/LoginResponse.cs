namespace Authorization.Core.Login;

public record LoginResponse(Contracts.User User, string AccessToken, string RefreshToken,  DateTime ExpiresAt);