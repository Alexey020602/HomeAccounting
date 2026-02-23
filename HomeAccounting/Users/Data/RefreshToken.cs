namespace HomeAccounting.Users.Data;

public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTimeOffset Expires { get; set; }
}