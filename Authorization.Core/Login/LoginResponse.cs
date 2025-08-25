namespace Authorization.Core.Login;

public record LoginResponse(Guid UserId, string Login, string AccessToken, string RefreshToken);