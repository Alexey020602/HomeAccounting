using BlazorShared.Authorization.Dto;
using BlazorShared.Utils;

namespace BlazorShared.Authorization;

public sealed class AuthenticationStorage(ILocalStorage localStorage): IAuthenticationStorage
{
    private SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
    private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);
    private readonly ILocalStorage _localStorage = localStorage;
    private const string AuthorizationKey = "Authorization";
    public async ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return;
        await _localStorage.RemoveAsync(AuthorizationKey, cancellationToken);
    }

    public async ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return;
        await _localStorage.SetAsync(AuthorizationKey, authorization, cancellationToken);
    }

    public async ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return null;
        return await _localStorage.GetAsync<Authentication>(AuthorizationKey, cancellationToken);
    }
}

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