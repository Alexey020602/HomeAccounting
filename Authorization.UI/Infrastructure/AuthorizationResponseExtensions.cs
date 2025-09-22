using Authorization.Contracts;
using Authorization.UI.Dto;

namespace Authorization.UI.Infrastructure;

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