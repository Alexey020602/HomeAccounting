using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Authorization.Extensions;
using Authorization.Models;
using Checks.DataBase.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using User = Authorization.Contracts.User;

namespace Authorization;

public class TokenService(IConfiguration configuration, ILogger<TokenService> logger): ITokenService
{
    // private const int ExpirationMinutes = 60;
    
    // private const string ValidIssuer = "ValidIssuer";
    // private const string ValidAudience = "ValidAudience";
    // private const string SymmetricSecurityKey = "SymmetricSecurityKey";
    private readonly SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    private JwtTokenSettings Settings => configuration.CreateJwtTokenSettings();
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