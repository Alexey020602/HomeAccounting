using BlazorConsolidated.Users.Infrastructure.Abstractions;
using BlazorConsolidated.Users.Infrastructure.Api;

namespace BlazorConsolidated.Users.Infrastructure;

internal sealed class TokenService(IAuthenticationStorage authenticationStorage, IAuthorizationApi authorizationApi): ITokenService
{
    public async Task<string?> GetFreshAccessToken(CancellationToken cancellationToken = default)
    {
        if (await authenticationStorage.GetAuthorizationAsync(cancellationToken) is not { } authentication)
        {
            return null;
        }
        if (authentication is { Expired: false })
        {
            return authentication.AccessToken;
        }

        await authenticationStorage.RemoveAuthorizationAsync(cancellationToken);
        
        return await GetRefreshedToken(cancellationToken);
    }
    public async Task<string> GetRefreshedToken(CancellationToken cancellationToken = default)
    {
        if (await authenticationStorage.GetAuthorizationAsync(cancellationToken) is not { } authentication)
        {
            throw new InvalidOperationException("No refresh token found");
        }
        
        return await GetRefreshedToken(authentication.RefreshToken, cancellationToken);
    }

    private async Task<string> GetRefreshedToken(string refreshToken, CancellationToken cancellationToken = default)
    {
        var authorizationResponse = await authorizationApi.RefreshToken(refreshToken);
        await authenticationStorage.SetAuthorizationAsync(authorizationResponse.ConvertToAuthentication(),
            cancellationToken);
        return authorizationResponse.AccessToken;
    }
}