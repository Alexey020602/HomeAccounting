using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DataBase.Entities;

public class User: IdentityUser<string>
{
    public RefreshToken RefreshToken { get; set; } = new();
}

[ComplexType]
public class RefreshToken
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
}