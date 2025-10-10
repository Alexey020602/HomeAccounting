using BlazorConsolidated.Users.Dto;
using ClientServerContracts.User.Login;

namespace BlazorConsolidated.Users.Infrastructure;

public static class AuthorizationResponseExtensions
{
    public static Authentication ConvertToAuthentication(this AuthorizationResponse response)
    {
        return new Authentication(
            response.AccessToken,
            response.RefreshToken,
            response.User,
            response.ExpiresAt
        );
    }
}