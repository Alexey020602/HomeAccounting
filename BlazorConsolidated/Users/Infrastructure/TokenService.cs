using BlazorConsolidated.Users.Dto;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Api.Users;
using ClientServerContracts.Users.Login;
using ClientServerContracts.Users.Refresh;

namespace BlazorConsolidated.Users.Infrastructure;

internal sealed class TokenService(IAuthenticationStorage authenticationStorage, IAuthorizationApi authorizationApi): ITokenService, IDisposable, IAsyncDisposable
{
    private SemaphoreSlim semaphore = new(1, 1);
    public async Task<string?> GetFreshAccessToken(CancellationToken cancellationToken = default)
    {
        if (await authenticationStorage.GetAuthorizationAsync(cancellationToken) is not { } authentication)
        {
            return null;
        }
        
        if (!authentication.AccessTokenExpired(DateTimeOffset.UtcNow))
        {
            return authentication.AccessToken;
        }

        // Не удаляем сразу, пытаемся обновить
        try
        {
            return (await RefreshToken(authentication).WaitAsync(cancellationToken)).AccessToken;
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            return null;
        }
    }
    public async Task<string> GetRefreshedToken(CancellationToken cancellationToken = default)
    {
        if (await authenticationStorage.GetAuthorizationAsync(cancellationToken) is not { } authentication)
        {
            throw new InvalidOperationException("No refresh token found");
        }

        // await authenticationStorage.RemoveAuthorizationAsync(cancellationToken);
        return (await ForceRefreshToken(authentication, CancellationToken.None).WaitAsync(cancellationToken)).AccessToken;
    }

    private async Task<Authentication> ForceRefreshToken(Authentication authentication,
        CancellationToken cancellationToken = default)
    {
        if(!await semaphore.WaitAsync(120*1000, cancellationToken))
            throw new InvalidOperationException("Refresh token operation timed out");
        
        try
        {
            // if (!forceRefresh){
            var storedAuthentication = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
            if (storedAuthentication != null && 
                storedAuthentication != authentication)
            {
                return storedAuthentication;
            }
            // }
            return await RefreshAuthentication(authentication, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }
    private async Task<Authentication> RefreshToken(Authentication authentication, CancellationToken cancellationToken = default)
    {
        if(!await semaphore.WaitAsync(120*1000, cancellationToken))
            throw new InvalidOperationException("Refresh token operation timed out");
        
        try
        {
            // if (!forceRefresh){
                var storedAuthentication = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
                if (storedAuthentication != null && 
                    !storedAuthentication.AccessTokenExpired(DateTimeOffset.UtcNow))
                {
                    return storedAuthentication;
                }
            // }
            return await RefreshAuthentication(authentication, cancellationToken);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task<Authentication> RefreshAuthentication(Authentication authentication, CancellationToken cancellationToken)
    {
        var request = new RefreshTokenRequest(authentication.AccessToken, authentication.RefreshToken);
        var authorizationResponse = await authorizationApi.RefreshToken(request, cancellationToken);

        var newAuthentication = authorizationResponse.ConvertToAuthentication(DateTimeOffset.UtcNow);
        await authenticationStorage.SetAuthorizationAsync(newAuthentication, cancellationToken);
        return newAuthentication;
    }

    public void Dispose()
    {
        semaphore.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        Dispose();
        return ValueTask.CompletedTask;
    }
}