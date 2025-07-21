using Microsoft.AspNetCore.Identity;

namespace Authorization.Core;

public class User: IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public RefreshToken? RefreshToken { get; set; }
}

