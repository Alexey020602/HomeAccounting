using Microsoft.EntityFrameworkCore;

namespace Authorization.DataBase;

[Owned]
public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
}