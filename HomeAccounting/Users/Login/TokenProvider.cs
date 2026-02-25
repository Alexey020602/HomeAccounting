using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Users.Login;

internal sealed class TokenProvider(IOptions<JwtTokenSettings> settings, ILogger<TokenProvider> logger): ITokenProvider
{
    // private const int ExpirationMinutes = 60;
    
    // private const string ValidIssuer = "ValidIssuer";
    // private const string ValidAudience = "ValidAudience";
    // private const string SymmetricSecurityKey = "SymmetricSecurityKey";
    private readonly SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    private JwtTokenSettings Settings => settings.Value;
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;
    public ClaimsPrincipal GetPrincipal(string token)
    {
        var principal = tokenHandler.ValidateToken(token, Settings.TokenValidationParameters, out var validatedToken);
        if (validatedToken is not JwtSecurityToken jwtSecurityToken || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
    public Data.RefreshToken CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return new()
        {
            Token = Convert.ToBase64String(randomNumber),
            Expires = Settings.RefreshTokenExpirationDate,
        };
    }
    public AccessToken CreateToken(IReadOnlyList<Claim> claims)
    {
        var token = CreateJwtSecurityToken(claims);
        var accessToken = tokenHandler.WriteToken(token);
        DateTimeOffset expiredAt = token.ValidTo;
        logger.LogInformation("JWT Token created");
        return new AccessToken(accessToken, expiredAt);
    }
    
    private JwtSecurityToken CreateJwtSecurityToken(IEnumerable<Claim> claims) =>
        new(
            Settings.Issuer,
            Settings.Audience,
            claims,
            expires: Settings.AccessTokenExpirationDate.UtcDateTime,
            signingCredentials: CreateSigningCredentials());
    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            Settings.SecurityKey,
            SecurityAlgorithm
        );
    }

    
}