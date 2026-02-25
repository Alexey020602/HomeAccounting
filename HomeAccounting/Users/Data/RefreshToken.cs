using HomeAccounting.Common.Model;

namespace HomeAccounting.Users.Data;

internal class RefreshToken: Entity<RefreshTokenId>
{
    public string Token { get; private set; }
    public DateTimeOffset Expires { get; private set; }
    public bool Invalidated { get; private set; }
    public JwtId JwtId { get; private set; }
    public UserId UserId { get; private set; }

    private RefreshToken() : base()
    {
        Token = string.Empty;
    }

    public RefreshToken(
        string token,
        DateTimeOffset expires,
        JwtId jwtId,
        UserId userId) : base()
    {
        Token = token;
        Expires = expires;
        JwtId = jwtId;
        UserId = userId;
    }
}