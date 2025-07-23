using Authorization.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Core;

public class User: IdentityUser<Guid>
{
    public string? FullName { get; set; }
    public RefreshToken? RefreshToken { get; set; }

    public static implicit operator Contracts.User(User user) => new Contracts.User(
        user.Id, 
        user.UserName ?? throw UserException.NoUserName);
}

