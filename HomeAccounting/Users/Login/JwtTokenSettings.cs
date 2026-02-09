using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace HomeAccounting.Users.Login;

public class JwtTokenSettings
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int AccessTokenExpireMinutes { get; init; }
    public required int RefreshTokenExpireDays { get; init; }
    public DateTimeOffset AccessTokenExpirationDate => DateTimeOffset.UtcNow.AddMinutes(AccessTokenExpireMinutes);
    public DateTimeOffset RefreshTokenExpirationDate => DateTimeOffset.UtcNow.AddDays(RefreshTokenExpireDays);
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