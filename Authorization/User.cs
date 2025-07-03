using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization;

public class User: IdentityUser<string>
{
    public RefreshToken? RefreshTokenToken { get; set; }
}
[Owned]
public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
}