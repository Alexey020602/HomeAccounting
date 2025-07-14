using Microsoft.AspNetCore.Identity;

namespace Authorization.Core;

public class User: IdentityUser<string>
{
    public RefreshToken? RefreshToken { get; set; }
}

