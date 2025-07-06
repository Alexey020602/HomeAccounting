using Authorization.UI.Dto;

namespace Authorization.UI;

public interface IAuthenticationStorage
{
    ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default);
    ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default);
    ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default);
}