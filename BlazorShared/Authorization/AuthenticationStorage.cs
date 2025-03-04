using BlazorShared.Authorization.Dto;
using BlazorShared.Utils;
using Microsoft.Extensions.Logging;

namespace BlazorShared.Authorization;

public sealed class AuthenticationStorage(ILocalStorage localStorage, ILogger<AuthenticationStorage> logger): IAuthenticationStorage
{
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);
    private readonly ILocalStorage _localStorage = localStorage;
    private const string AuthorizationKey = "Authorization";
    public async ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return;
        try
        {
            logger.LogInformation("Removing Authorization");
            await _localStorage.RemoveAsync(AuthorizationKey, cancellationToken);
            logger.LogInformation("Authorization removed");
        }
        finally
        {
            ReleaseSemaphore();
        }
    }

    public async ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return;
        try
        {
            logger.LogInformation("Setting Authorization {authorization}", authorization);
            await _localStorage.SetAsync(AuthorizationKey, authorization, cancellationToken);
            logger.LogInformation("Authorization set");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set Authorization {authorization}", authorization);
            throw;
        }
        finally
        {
            ReleaseSemaphore();
        }
    }

    public async ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return null;

        try
        {
            logger.LogInformation("Getting Authorization");
            var authentication = await _localStorage.GetAsync<Authentication>(AuthorizationKey, cancellationToken);
            logger.LogInformation("Authorization retrieved {authorization}", authentication);
            return authentication;
        }
        finally
        {
            ReleaseSemaphore();
        }
    }

    private void ReleaseSemaphore()
    {
        semaphore.Release();
        logger.LogInformation("Semaphore released");
    }
}

// public static class SemaphoreSlimExtensions
// {
//     public static async Task<T?> Perform<T>(this SemaphoreSlim semaphore, Action<Task<T>> action, CancellationToken cancellationToken = default)
//     {
//         
//     }
// }

public sealed class MemoryAuthenticationStorage : IAuthenticationStorage
{
    private readonly Lock locker = new Lock();
    private Authentication? authorization;

    public ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        using var _ = locker.EnterScope();
        authorization = null;
        return ValueTask.CompletedTask;
    }

    public ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default)
    {
        using var _ = locker.EnterScope();
        this.authorization = authorization;
        return ValueTask.CompletedTask;
    }

    public ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        using var _ = locker.EnterScope();
        return ValueTask.FromResult(authorization);
    }
}