using System.Collections.Concurrent;
using BlazorConsolidated.Users.Dto;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Api.Users;
using ClientServerContracts.Users.Refresh;

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
        
        if (!authentication.AccessTokenExpired(DateTimeOffset.UtcNow))
        {
            return authentication.AccessToken;
        }

        // Не удаляем сразу, пытаемся обновить
        try
        {
            return await GetRefreshedTokenTask(authentication).WaitAsync(cancellationToken);
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
        
        return await GetRefreshedTokenTask(authentication).WaitAsync(cancellationToken);
    }

    private Task<string> GetRefreshedTokenTask(Authentication authentication)
    {
        var key = authentication.RefreshToken;
        using var enterScope = @lock.EnterScope();

        if (refreshTokenTasks.TryGetValue(key, out var existingTask)) return existingTask;

        var baseTask = RefreshTokenCore(authentication);
        var task = ClearKeyOnComplete(baseTask, key);
        refreshTokenTasks[key] = task;

        // _ = task.ContinueWith((t, state) =>
        //     {
        //         var (tokenKey, dictionary) = ((string, ConcurrentDictionary<string, Task<string>>))state!;
        //         using var scope = @lock.EnterScope();
        //         if (dictionary.TryGetValue(tokenKey, out var completedTask) && completedTask == t)
        //             dictionary.TryRemove(tokenKey, out _);
        //     },
        //     (key, refreshTokenTasks),
        //     TaskScheduler.Default
        // );
        return task;
    }
    
    

    private async Task<string> RefreshTokenCore(Authentication authentication)
    {
        var request = new RefreshTokenRequest(authentication.AccessToken, authentication.RefreshToken);
        var authorizationResponse = await authorizationApi.RefreshToken(request);
        await authenticationStorage.SetAuthorizationAsync(authorizationResponse.ConvertToAuthentication(DateTimeOffset.UtcNow));
        return authorizationResponse.AccessToken;
    }

    private async Task<string> ClearKeyOnComplete(Task<string> task, string key)
    {
        try
        {
            return await task;
        }
        finally
        {
            using var scope = @lock.EnterScope();
            if (refreshTokenTasks.TryGetValue(key, out var existingTask) && existingTask == task)
                refreshTokenTasks.TryRemove(key, out _);
        }
    }
}