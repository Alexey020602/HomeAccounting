using Microsoft.AspNetCore.Identity;

namespace Authorization.DataBase;

public class User: IdentityUser<string>
{
    public RefreshToken? RefreshTokenToken { get; set; }
}