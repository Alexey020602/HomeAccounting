using BlazorShared.Authorization.Dto;
using BlazorShared.Utils;

namespace BlazorShared.Authorization;

public sealed class AuthenticationStorage(ILocalStorage localStorage): IAuthenticationStorage
{
    private readonly ILocalStorage _localStorage = localStorage;
    private const string AuthorizationKey = "Authorization";
    public ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default) => _localStorage.RemoveAsync(AuthorizationKey, cancellationToken);

    public ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default) =>
        _localStorage.SetAsync(AuthorizationKey, authorization, cancellationToken);

    public ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default) =>
        _localStorage.GetAsync<Authentication>(AuthorizationKey, cancellationToken);
}