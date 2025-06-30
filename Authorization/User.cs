using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Authorization;

public class User: IdentityUser<string>
{
    public static User Default => new User { Id = "00000000-0000-0000-0000-000000000000", UserName = "Пользователь не выбран" };
    // public string? RefreshToken { get; set; }
    // public DateTime? RefreshTokenExpiry { get; set; }

    // [NotMapped]
    public RefreshToken? RefreshTokenToken { get; set; }
    // {
    //     get
    //     {
    //         if (RefreshToken is null || !RefreshTokenExpiry.HasValue) return null;
    //         return new RefreshToken { Token = RefreshToken, Expires = RefreshTokenExpiry.Value,};
    //     }
    //     set
    //     {
    //         RefreshToken = value?.Token;
    //         RefreshTokenExpiry = value?.Expires;
    //     }
    // }
}
[Owned]
public class RefreshToken
{
    public required string Token { get; set; }
    public required DateTime Expires { get; set; }
}