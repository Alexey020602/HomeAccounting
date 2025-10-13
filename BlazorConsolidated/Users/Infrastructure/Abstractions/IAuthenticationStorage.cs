using BlazorConsolidated.Users.Dto;

namespace BlazorConsolidated.Users.Infrastructure.Abstractions;

public interface IAuthenticationStorage
{
    ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default);
    ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default);
    ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default);
}