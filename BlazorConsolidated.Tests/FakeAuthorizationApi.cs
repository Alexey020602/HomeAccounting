using ClientServerContracts.Api.Users;
using ClientServerContracts.Users.GetUser;
using ClientServerContracts.Users.Login;
using ClientServerContracts.Users.Register;

namespace BlazorConsolidated.Tests;

/// <summary>
/// Fake implementation of <see cref="IAuthorizationApi"/> for unit tests.
/// Only <see cref="RefreshToken"/> is implemented; other methods throw <see cref="NotImplementedException"/>.
/// </summary>
public sealed class FakeAuthorizationApi : IAuthorizationApi
{
    private readonly Func<string, AuthorizationResponse>? _refreshTokenResponse;
    private readonly Func<string, CancellationToken, Task<AuthorizationResponse>>? _refreshTokenTaskResponse;
    private readonly Exception? _refreshTokenException;
    private int _refreshTokenCallCount;

    public FakeAuthorizationApi(
        AuthorizationResponse? refreshTokenResponse = null,
        Exception? refreshTokenException = null)
    {
        _refreshTokenResponse = refreshTokenResponse is not null ? _ => refreshTokenResponse : null;
        _refreshTokenException = refreshTokenException;
    }

    public FakeAuthorizationApi(Func<string, AuthorizationResponse> refreshTokenResponse)
    {
        _refreshTokenResponse = refreshTokenResponse;
        _refreshTokenException = null;
    }

    /// <summary>
    /// Use when the test needs to control when the refresh completes (e.g. cancellation tests).
    /// Returns the task without blocking the calling thread.
    /// </summary>
    public FakeAuthorizationApi(Func<string, CancellationToken, Task<AuthorizationResponse>> refreshTokenTaskResponse)
    {
        _refreshTokenTaskResponse = refreshTokenTaskResponse;
    }

    public int RefreshTokenCallCount => _refreshTokenCallCount;

    public Task<bool> CheckLoginExist(CheckLoginExistQueryParameters query, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<AuthorizationResponse> Login(LoginRequest loginRequest, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task Register(RegistrationRequest registrationRequest, CancellationToken cancellationToken = default) =>
        throw new NotImplementedException();

    public Task<AuthorizationResponse> RefreshToken(string refreshToken, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        Interlocked.Increment(ref _refreshTokenCallCount);
        if (_refreshTokenException is not null)
            throw _refreshTokenException;
        if (_refreshTokenTaskResponse is not null)
            return _refreshTokenTaskResponse(refreshToken, cancellationToken);
        if (_refreshTokenResponse is not null)
            return Task.FromResult(_refreshTokenResponse(refreshToken));
        return Task.FromResult(CreateDefaultResponse(refreshToken));
    }

    private static AuthorizationResponse CreateDefaultResponse(string refreshToken)
    {
        var user = new User(Guid.NewGuid(), "test", "Test User");
        return new AuthorizationResponse(
            "Bearer",
            user,
            "new-access-token",
            "new-refresh-token",
            DateTimeOffset.UtcNow.AddHours(1));
    }

    /// <summary>
    /// Creates a response with the given access token (e.g. to assert same token in deduplication test).
    /// </summary>
    public static AuthorizationResponse ResponseWithAccessToken(string accessToken, string refreshToken = "rt")
    {
        var user = new User(Guid.NewGuid(), "test", "Test User");
        return new AuthorizationResponse(
            "Bearer",
            user,
            accessToken,
            refreshToken,
            DateTimeOffset.UtcNow.AddHours(1));
    }

    /// <summary>
    /// Creates a fake that returns a different response per call (by call count).
    /// </summary>
    public static FakeAuthorizationApi WithSequentialResponses(params AuthorizationResponse[] responses)
    {
        var index = -1;
        return new FakeAuthorizationApi(_ => responses[Interlocked.Increment(ref index)]);
    }
}
