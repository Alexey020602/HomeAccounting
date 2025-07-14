namespace Authorization.Core;

public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
}