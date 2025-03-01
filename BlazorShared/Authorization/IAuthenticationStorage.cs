using BlazorShared.Authorization.Dto;

namespace BlazorShared.Authorization;

public interface IAuthenticationStorage
{
    ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default);
    ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default);
    ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default);
}