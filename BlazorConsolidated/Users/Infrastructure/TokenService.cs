using System.Collections.Concurrent;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Api.Users;

namespace BlazorConsolidated.Users.Infrastructure;

internal sealed class TokenService(IAuthenticationStorage authenticationStorage, IAuthorizationApi authorizationApi): ITokenService
{
    private readonly ConcurrentDictionary<string, Task<string>> refreshTokenTasks = new();
    private readonly Lock @lock = new();
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

        // Не удаляем сразу, пытаемся обновить
        try
        {
            return await GetRefreshedTokenTask(authentication.RefreshToken).WaitAsync(cancellationToken);
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
        
        return await GetRefreshedTokenTask(authentication.RefreshToken).WaitAsync(cancellationToken);
    }

    private Task<string> GetRefreshedTokenTask(string refreshToken)
    {
        using var enterScope = @lock.EnterScope();
        
        if (refreshTokenTasks.TryGetValue(refreshToken, out var existingTask)) return existingTask;
        
        
        var task = RefreshTokenCore(refreshToken);
        
        refreshTokenTasks[refreshToken] = task;

        _ = task.ContinueWith((t, state) =>
            {
                var (token, dictionary) = ((string, ConcurrentDictionary<string, Task<string>>))state!;
                using var scope = @lock.EnterScope();
                if  (dictionary.TryGetValue(token, out var completedTask) && completedTask == t)
                    dictionary.TryRemove(token, out _);
            },
            (refreshToken, refreshTokenTasks),
            TaskScheduler.Default
        );
        return task;
    }

    private async Task<string> RefreshTokenCore(string refreshToken)
    {
        var authorizationResponse = await authorizationApi.RefreshToken(refreshToken);
        await authenticationStorage.SetAuthorizationAsync(authorizationResponse.ConvertToAuthentication());
        return authorizationResponse.AccessToken;
    }
    
}