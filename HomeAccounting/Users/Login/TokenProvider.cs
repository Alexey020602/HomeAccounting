using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ClientServerShared.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Users.Login;

internal sealed class TokenProvider(IOptions<JwtTokenSettings> settings, UsersContext usersContext): ITokenProvider
{
    private const int SecondsInMinute = 60;
    private const int SecondsInDay = 60 * 60 * 24;
    private readonly SecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
    private JwtTokenSettings Settings => settings.Value;
    private const string SecurityAlgorithm = SecurityAlgorithms.HmacSha256;

    private static string HashRefreshToken(string refreshToken)
    {
        var refreshTokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        using var sha256 = SHA256.Create();
        
        return GetHash(sha256, refreshTokenBytes);
    }

    private static string GetHash(HashAlgorithm hashAlgorithm, byte[] input)
    {
        var data = hashAlgorithm.ComputeHash(input);
        return Convert.ToHexString(data);
    }
    public async Task<TokenResult> CreateToken(User user, CancellationToken cancellationToken)
    {
        var sessionId = SessionId.CreateNew();
        
        var newToken = GenerateJwtToken(user);
        var (newRefreshToken, newRefreshTokenString) = GenerateRefreshToken(newToken, sessionId, user);
        
        usersContext.RefreshTokens.Add(newRefreshToken);
        await usersContext.SaveChangesAsync(cancellationToken);
        var newTokenValue = tokenHandler.WriteToken(newToken);
        return new TokenResult(
            user.Id,
            newTokenValue,
            newRefreshTokenString,
            Settings.AccessTokenExpireMinutes * SecondsInMinute,
            Settings.RefreshTokenExpireDays * SecondsInDay
        );
    }

    public async Task<TokenResult> RefreshToken(string token, string refreshToken, CancellationToken cancellationToken)
    {
        var principal = GetPrincipal(token);
        
        var jwtId = GetJwtIdFromPrincipal(principal);

        var refreshTokenHash = HashRefreshToken(refreshToken);
        var storedRefreshToken = await usersContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshTokenHash, cancellationToken);

        if (storedRefreshToken is null)
        {
            throw new SecurityTokenException("Refresh token not found");
        }

        if (!storedRefreshToken.IsActual(DateTimeOffset.UtcNow))
        {
            //todo Подумать над тем, чтобы добавить отзыв сессии при истечении токена
            throw new SecurityTokenException("Refresh token has expired");
        }

        if (storedRefreshToken.IsUsed())
        {
            await DeleteSessionsTokens(storedRefreshToken.SessionId, cancellationToken);
            throw new SecurityTokenException("Refresh token has been used");
        }

        if (storedRefreshToken.JwtId != jwtId)
        {
            throw new SecurityTokenException("Invalid token");
        }

        var user = await usersContext.Users.FindAsync([storedRefreshToken.UserId], cancellationToken);

        if (user is null)
        {
            throw new SecurityTokenException("User not found");
        }

        
        var newToken = GenerateJwtToken(user);
        var (newRefreshToken, newRefreshTokenString) = GenerateRefreshToken(newToken, storedRefreshToken.SessionId, user);
        storedRefreshToken.Use(DateTimeOffset.UtcNow);
        usersContext.RefreshTokens.Add(newRefreshToken);
        usersContext.RefreshTokens.Update(storedRefreshToken);
        await usersContext.SaveChangesAsync(cancellationToken);
        var newTokenValue = tokenHandler.WriteToken(newToken);
        return new TokenResult(
            user.Id,
            newTokenValue,
            newRefreshTokenString,
            Settings.AccessTokenExpireMinutes * SecondsInMinute,
            Settings.RefreshTokenExpireDays * SecondsInDay
        );
    }

    private static JwtId GetJwtIdFromPrincipal(ClaimsPrincipal principal)
    {
        var jtiValue = principal.Claims.SingleOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (jtiValue is null || !Guid.TryParse(jtiValue, out var jti))
        {
            throw new SecurityTokenException("Token not contains jti");
        }
        
        var jwtId = new JwtId(jti);
        return jwtId;
    }

    /// <summary>
    /// Revokes the session associated with the given refresh token. Idempotent: if the token is unknown or already revoked, completes without error.
    /// </summary>
    public async Task LogoutUserSession(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenHash = HashRefreshToken(refreshToken);
        var stored = await usersContext.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == refreshTokenHash, cancellationToken);
        if (stored is null)
            return;

        await usersContext.RefreshTokens
            .Where(rt => rt.SessionId == stored.SessionId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    /// <summary>
    /// Revokes all sessions of the user associated with the given refresh token. Idempotent: if the token is unknown or already revoked, completes without error.
    /// </summary>
    public async Task LogoutAllUserSessions(string refreshToken, CancellationToken cancellationToken)
    {
        var refreshTokenHash = HashRefreshToken(refreshToken);
        var stored = await usersContext.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(rt => rt.Token == refreshTokenHash, cancellationToken);
        if (stored is null)
            return;

        await usersContext.RefreshTokens
            .Where(rt => rt.UserId == stored.UserId)
            .ExecuteDeleteAsync(cancellationToken);
    }
    private async Task DeleteSessionsTokens(SessionId sessionId, CancellationToken cancellationToken)
    {
        var tokens = await usersContext.RefreshTokens.Where(rt => rt.SessionId == sessionId)
            .ExecuteDeleteAsync(cancellationToken);
        if (tokens == 0)
        {
            throw new InvalidOperationException($"No tokens deleted from session {sessionId}");
        }
    }

    private ClaimsPrincipal GetPrincipal(string token)
    {
        var principal = tokenHandler.ValidateToken(token, Settings.TokenValidationParameters, out var validatedToken);
        if (validatedToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithm, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
    private (Data.RefreshToken, string) GenerateRefreshToken(JwtSecurityToken token, SessionId sessionId, User user)
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        var refreshTokenString = Convert.ToBase64String(randomNumber);
        var newRefreshTokenValue = HashRefreshToken(refreshTokenString);

        if (!Guid.TryParse(token.Id, out var jti))
        {
            throw new SecurityTokenException("Invalid token");
        }
        
        var jwtId = new JwtId(jti);

        var refreshToken = new Data.RefreshToken(
            newRefreshTokenValue,
            Settings.RefreshTokenExpirationDate,
            jwtId,
            user.Id,
            sessionId
        );
        return (refreshToken, refreshTokenString);
    }

    private JwtSecurityToken GenerateJwtToken(User user)
    {
        var jti = Guid.CreateVersion7();
        var userClaims = user.GetClaims();
        IEnumerable<Claim> claims = [
            new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
            .. userClaims
        ];
        var signingCredentials = new SigningCredentials(
            Settings.SecurityKey,
            SecurityAlgorithm
        );
        var token = new JwtSecurityToken(
            Settings.Issuer,
            Settings.Audience,
            claims,
            expires: Settings.AccessTokenExpirationDate.UtcDateTime,
            signingCredentials: signingCredentials);
        
        return token;
    }
}