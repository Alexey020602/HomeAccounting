using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Authorization.Core;

public class JwtTokenSettings
{
    public const string SectionName = "JwtTokenSettings";
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int AccessTokenExpireMinutes { get; init; }
    public required int RefreshTokenExpireDays { get; init; }
    public DateTime AccessTokenExpirationDate => DateTime.UtcNow.AddMinutes(AccessTokenExpireMinutes);
    public DateTime RefreshTokenExpirationDate => DateTime.UtcNow.AddDays(RefreshTokenExpireDays);
    public SecurityKey SecurityKey  => new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(Key)
    );
    public TokenValidationParameters TokenValidationParameters => new()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = false,
        IssuerSigningKey = SecurityKey,
        ValidateLifetime = true,
        
    };
}