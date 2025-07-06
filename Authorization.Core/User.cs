namespace Authorization.Core;

public class UnregisteredUser
{
    public required string Login { get; init; }
    public required string UserName { get; init; }
}
public class User
{
    public required string Login { get; init; }
    public required string Username { get; init; }
    // public required string Password { get; init; }
    public required RefreshToken RefreshToken { get; init; }
}

public class RefreshToken
{
    public required string Token { get; init; }
    public required DateTime Expires { get; init; } 
}