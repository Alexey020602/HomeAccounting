using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Authorization;

public interface ITokenService
{
    
}
public class TokenService(IConfiguration configuration, ILogger<TokenService> logger): ITokenService
{
    private const int ExpirationMinutes = 60;
    private const string SectionName = "JwtTokenSettings";
    private const string ValidIssuer = "ValidIssuer";
    private const string ValidAudience = "ValidAudience";
    private const string SymmetricSecurityKey = "SymmetricSecurityKey";
    private readonly SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    
    public string CreateToken(User user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = CreateJwtSecurityToken(CreateClaims(user), CreateSigningCredentials(), expiration);
        logger.LogInformation("JWT Token created");
        return tokenHandler.WriteToken(token);
    }
    
    private JwtSecurityToken CreateJwtSecurityToken(IEnumerable<Claim> claims, SigningCredentials signingCredentials,
        DateTime expiration)
    {
        return new JwtSecurityToken(
            configuration.GetSection(SectionName)[ValidIssuer],
            configuration.GetSection(SectionName)[ValidAudience],
            claims,
            expires: expiration,
            signingCredentials: signingCredentials);
    }

    private IEnumerable<Claim> CreateClaims(User user) => CreateUserClaims(user);

    private IEnumerable<Claim> CreateUserClaims(User user) =>
    [
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Name!)
    ];

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration.GetSection(SectionName)[SymmetricSecurityKey]!)
            ),
            SecurityAlgorithms.HmacSha256
        );
    }
}