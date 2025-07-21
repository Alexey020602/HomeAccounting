using Authorization.UI.Dto;
using Shared.Blazor;

namespace Authorization.UI;

public sealed class AuthenticationStorage(ILocalStorage localStorage) : IAuthenticationStorage
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
            var authentication = await localStorage.GetAsync<Authentication>(AuthorizationKey, cancellationToken);
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
    }
}