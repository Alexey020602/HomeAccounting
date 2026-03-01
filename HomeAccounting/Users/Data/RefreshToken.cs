using HomeAccounting.Common.Model;

namespace HomeAccounting.Users.Data;

internal record struct SessionId(Guid Value)
{
    public static SessionId CreateNew() => new(Guid.CreateVersion7());
}

internal class RefreshToken: Entity<RefreshTokenId>
{
    public string Token { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public DateTimeOffset? UsedAt { get; private set; }
    public DateTimeOffset? RevokedAt { get; private set; }
    public JwtId JwtId { get; private set; }
    public UserId UserId { get; private set; }
    public SessionId SessionId { get; private set; }
    private RefreshToken() : base()
    {
        Token = string.Empty;
    }

    public RefreshToken(
        string token,
        DateTimeOffset expiresAt,
        JwtId jwtId,
        UserId userId,
        SessionId sessionId) : base()
    {
        Token = token;
        ExpiresAt = expiresAt;
        JwtId = jwtId;
        UserId = userId;
        SessionId = sessionId;
    }
    
    public void Revoke(DateTimeOffset revokedAt) => RevokedAt = revokedAt;
    public void Use(DateTimeOffset usedAt) => UsedAt = usedAt;
    public bool IsActual(DateTimeOffset currentDate) => ExpiresAt > currentDate;
    public bool IsUsed() => UsedAt != null;
    public bool IsRevoked() => RevokedAt != null;
}