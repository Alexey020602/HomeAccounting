using BlazorConsolidated.Users.Dto;
using ClientServerContracts.Users.Login;

namespace BlazorConsolidated.Users.Infrastructure;

public static class AuthorizationResponseExtensions
{
    public static Authentication ConvertToAuthentication(this TokenResponse response, DateTimeOffset pointTime)
    {
        var startExpireDate = pointTime.AddSeconds(-30);
        
        return new Authentication(
            response.AccessToken,
            response.RefreshToken,
            response.User,
            startExpireDate.AddSeconds(response.ExpiresIn),
            startExpireDate.AddSeconds(response.RefreshExpiresIn)
        );
    }
}