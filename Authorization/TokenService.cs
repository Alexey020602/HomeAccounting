using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Authorization;

public class TokenService(IConfiguration configuration, ILogger<TokenService> logger): ITokenService
{
    // private const int ExpirationMinutes = 60;
    private const string SectionName = "JwtTokenSettings";
    // private const string ValidIssuer = "ValidIssuer";
    // private const string ValidAudience = "ValidAudience";
    // private const string SymmetricSecurityKey = "SymmetricSecurityKey";
    private readonly SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    private JwtTokenSettings Settings => configuration.GetSection(SectionName).Get<JwtTokenSettings>() ?? throw new Exception("Missing JwtTokenSettings in app.config");
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
    public string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
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
            expires: Settings.ExpirationDate,
            signingCredentials: CreateSigningCredentials());

    private static IEnumerable<Claim> CreateClaims(User user) => CreateUserClaims(user);

    private static IEnumerable<Claim> CreateUserClaims(User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name!)
    ];

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            Settings.SecurityKey,
            SecurityAlgorithm
        );
    }

    
}