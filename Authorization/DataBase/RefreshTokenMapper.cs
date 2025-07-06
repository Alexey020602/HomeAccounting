namespace Authorization.DataBase;

public static class RefreshTokenMapper
{
    public static RefreshToken ConvertToRefreshToken(this Core.RefreshToken refreshToken) => new RefreshToken()
    {
        Token = refreshToken.Token,
        Expires = refreshToken.Expires,
    };

    public static Core.RefreshToken ConvertToCoreRefreshToken(this RefreshToken refreshToken) => new Core.RefreshToken()
    {
        Token = refreshToken.Token,
        Expires = refreshToken.Expires,
    };
}