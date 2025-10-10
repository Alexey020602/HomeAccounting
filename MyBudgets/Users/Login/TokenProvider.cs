using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MyBudgets.Users.Data;

namespace MyBudgets.Users.Login;

public class TokenProvider(IOptions<JwtTokenSettings> settings, ILogger<TokenProvider> logger): ITokenProvider
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
    public RefreshToken CreateRefreshToken()
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
    public string CreateToken(IReadOnlyList<Claim> claims)
    {
        var token = CreateJwtSecurityToken(claims);
        logger.LogInformation("JWT Token created");
        return tokenHandler.WriteToken(token);
    }
    
    private JwtSecurityToken CreateJwtSecurityToken(IEnumerable<Claim> claims) =>
        new(
            Settings.Issuer,
            Settings.Audience,
            claims,
            expires: Settings.AccessTokenExpirationDate,
            signingCredentials: CreateSigningCredentials());
    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            Settings.SecurityKey,
            SecurityAlgorithm
        );
    }

    
}