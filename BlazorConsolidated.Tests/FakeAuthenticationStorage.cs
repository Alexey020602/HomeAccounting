using BlazorConsolidated.Users.Dto;
using BlazorConsolidated.Users.Infrastructure.Abstractions;

namespace BlazorConsolidated.Tests;

/// <summary>
/// Fake implementation of <see cref="IAuthenticationStorage"/> for unit tests.
/// Stores a single authentication; <see cref="SetAuthorizationAsync"/> overwrites it.
/// </summary>
public sealed class FakeAuthenticationStorage : IAuthenticationStorage
{
    private Authentication? _current;

    public ValueTask<Authentication?> GetAuthorizationAsync(CancellationToken cancellationToken = default) =>
        ValueTask.FromResult(_current);

    public ValueTask SetAuthorizationAsync(Authentication authorization, CancellationToken cancellationToken = default)
    {
        _current = authorization;
        return ValueTask.CompletedTask;
    }

    public ValueTask RemoveAuthorizationAsync(CancellationToken cancellationToken = default)
    {
        _current = null;
        return ValueTask.CompletedTask;
    }

    /// <summary>
    /// Sets the current authentication (for test setup). Same effect as after <see cref="SetAuthorizationAsync"/>.
    /// </summary>
    public void SetCurrent(Authentication? authentication) => _current = authentication;
}
