using Microsoft.AspNetCore.Identity;

namespace DataBase.Entities;

public class User: IdentityUser<string>
{
    public string? RefreshToken { get; set; }
}