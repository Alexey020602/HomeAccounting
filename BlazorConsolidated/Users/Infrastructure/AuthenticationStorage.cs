using BlazorConsolidated.Common;
using BlazorConsolidated.Users.Dto;
using BlazorConsolidated.Users.Infrastructure.Abstractions;

namespace BlazorConsolidated.Users.Infrastructure;

public sealed class AuthenticationStorage(ILocalStorage localStorage) : IAuthenticationStorage, IDisposable
{
    private const string AuthorizationKey = "Authorization";
    private readonly ILocalStorage localStorage = localStorage;
    private readonly TimeSpan timeout = TimeSpan.FromSeconds(5);
    private readonly SemaphoreSlim semaphore = new(1, 1);

    public async ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return;
        try
        {
            await localStorage.RemoveAsync(AuthorizationKey, cancellationToken);
        }
        finally
        {
            ReleaseSemaphore();
        }
    }

    public async ValueTask SetAuthorizationAsync(Authentication authorization,
        CancellationToken cancellationToken = default)
    {
        if (!await semaphore.WaitAsync(timeout, cancellationToken).ConfigureAwait(false))
            return;
        try
        {
            await localStorage.SetAsync(AuthorizationKey, authorization, cancellationToken);
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
            return await localStorage.GetAsync<Authentication>(AuthorizationKey, cancellationToken);
        }
        finally
        {
            ReleaseSemaphore();
        }
    }

    private void ReleaseSemaphore()
    {
        semaphore.Release();
    }

    public void Dispose() => semaphore.Dispose();
}