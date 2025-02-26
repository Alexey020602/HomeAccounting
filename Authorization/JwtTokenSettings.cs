using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authorization;

public class JwtTokenSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int AccessTokenExpireMinutes { get; init; }
    public required int RefreshTokenExpireDays { get; init; }

    public static JwtTokenSettings Default => new() 
    {
        Issuer = "ValidIssuer",
        Audience = "ValidAudience",
        Key = "SymmetricSecurityKey",
        AccessTokenExpireMinutes = 60,
        RefreshTokenExpireDays = 3,
    };
    
    public DateTime ExpirationDate => DateTime.UtcNow.AddMinutes(AccessTokenExpireMinutes);
    public SecurityKey SecurityKey  => new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(Key)
    );
    public TokenValidationParameters TokenValidationParameters => new()
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = SecurityKey,
        ValidateLifetime = true,
    };
}