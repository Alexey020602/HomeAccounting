using BlazorConsolidated.Users.Dto;
using BlazorConsolidated.Users.Infrastructure;
using BlazorConsolidated.Users.Infrastructure.Abstractions;
using ClientServerContracts.Api.Users;
using ClientServerContracts.Users.GetUser;
using ClientServerContracts.Users.Login;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Tests;

public class TokenServiceTests
{
    private static readonly User TestUser = new(Guid.NewGuid(), "test", "Test User");

    private static Authentication NotExpiredAuth(string accessToken = "access", string refreshToken = "refresh") =>
        new(accessToken, refreshToken, TestUser, DateTimeOffset.UtcNow.AddHours(1), DateTimeOffset.UtcNow.AddDays(30));

    private static Authentication ExpiredAuth(string accessToken = "old-access", string refreshToken = "refresh") =>
        new(accessToken, refreshToken, TestUser, DateTimeOffset.UtcNow.AddSeconds(-1), DateTimeOffset.UtcNow.AddDays(30));

    private static ITokenService CreateTokenService(
        FakeAuthenticationStorage storage,
        FakeAuthorizationApi api)
    {
        var services = new ServiceCollection()
            .AddSingleton<IAuthenticationStorage>(storage)
            .AddSingleton<IAuthorizationApi>(api)
            .AddSingleton<ITokenService, TokenService>();
        return services.BuildServiceProvider().GetRequiredService<ITokenService>();
    }

    [Fact]
    public async Task GetFreshAccessToken_NoAuthorization_ReturnsNull()
    {
        var storage = new FakeAuthenticationStorage();
        var api = new FakeAuthorizationApi();
        var sut = CreateTokenService(storage, api);

        var result = await sut.GetFreshAccessToken();

        Assert.Null(result);
        Assert.Equal(0, api.RefreshTokenCallCount);
    }

    [Fact]
    public async Task GetFreshAccessToken_NotExpired_ReturnsCurrentAccessToken_NoRefresh()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(NotExpiredAuth("current-token", "rt"));
        var api = new FakeAuthorizationApi();
        var sut = CreateTokenService(storage, api);

        var result = await sut.GetFreshAccessToken();

        Assert.Equal("current-token", result);
        Assert.Equal(0, api.RefreshTokenCallCount);
    }

    [Fact]
    public async Task GetFreshAccessToken_Expired_RefreshSucceeds_ReturnsNewAccessToken()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(ExpiredAuth("old", "refresh-1"));
        var response = FakeAuthorizationApi.ResponseWithAccessToken("new-token", "refresh-1");
        var api = new FakeAuthorizationApi(response);
        var sut = CreateTokenService(storage, api);

        var result = await sut.GetFreshAccessToken();

        Assert.Equal("new-token", result);
        Assert.Equal(1, api.RefreshTokenCallCount);
        var after = await storage.GetAuthorizationAsync();
        Assert.NotNull(after);
        Assert.Equal("new-token", after.AccessToken);
    }

    [Fact]
    public async Task GetFreshAccessToken_Expired_RefreshThrows_ReturnsNull()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(ExpiredAuth("old", "rt"));
        var api = new FakeAuthorizationApi(refreshTokenException: new InvalidOperationException("refresh failed"));
        var sut = CreateTokenService(storage, api);

        var result = await sut.GetFreshAccessToken();

        Assert.Null(result);
        Assert.Equal(1, api.RefreshTokenCallCount);
    }

    [Fact]
    public async Task GetFreshAccessToken_Expired_CancelledDuringWait_ThrowsOperationCanceledException()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(ExpiredAuth("old", "rt"));
        var tcs = new TaskCompletionSource<TokenResponse>();
        var api = new FakeAuthorizationApi((_, ct) =>
        {
            ct.ThrowIfCancellationRequested();
            return tcs.Task;
        });
        var sut = CreateTokenService(storage, api);
        using var cts = new CancellationTokenSource();

        var task = sut.GetFreshAccessToken(cts.Token);
        await Task.Delay(50);
        await cts.CancelAsync();

        await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        tcs.SetResult(FakeAuthorizationApi.ResponseWithAccessToken("ignored", "rt"));
    }

    [Fact]
    public async Task GetRefreshedToken_NoAuthorization_ThrowsInvalidOperationException()
    {
        var storage = new FakeAuthenticationStorage();
        var api = new FakeAuthorizationApi();
        var sut = CreateTokenService(storage, api);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await sut.GetRefreshedToken());

        Assert.Equal("No refresh token found", ex.Message);
        Assert.Equal(0, api.RefreshTokenCallCount);
    }

    [Fact]
    public async Task GetRefreshedToken_HasAuthorization_CallsRefresh_ReturnsNewAccessToken()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(NotExpiredAuth("any", "refresh"));
        var response = FakeAuthorizationApi.ResponseWithAccessToken("refreshed-token", "refresh-2");
        var api = new FakeAuthorizationApi(response);
        var sut = CreateTokenService(storage, api);

        var result = await sut.GetRefreshedToken();

        Assert.Equal("refreshed-token", result);
        Assert.Equal(1, api.RefreshTokenCallCount);
        var after = await storage.GetAuthorizationAsync();
        Assert.NotNull(after);
        Assert.Equal("refreshed-token", after.AccessToken);
    }

    [Fact]
    public async Task GetRefreshedToken_CancelledDuringWait_ThrowsOperationCanceledException()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(NotExpiredAuth("any", "rt"));
        var tcs = new TaskCompletionSource<TokenResponse>();
        var api = new FakeAuthorizationApi((_, ct) =>
        {
            ct.ThrowIfCancellationRequested();
            return tcs.Task;
        });
        var sut = CreateTokenService(storage, api);
        using var cts = new CancellationTokenSource();

        var task = sut.GetRefreshedToken(cts.Token);
        await Task.Delay(50);
        await cts.CancelAsync();

        await Assert.ThrowsAsync<TaskCanceledException>(async () => await task);
        // tcs.SetResult(FakeAuthorizationApi.ResponseWithAccessToken("ignored", "rt"));
    }

    [Fact]
    public async Task GetFreshAccessToken_ConcurrentCallsWithSameExpiredToken_RefreshCalledOnce_AllGetSameToken()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(ExpiredAuth("old", "same-rt"));
        var response = FakeAuthorizationApi.ResponseWithAccessToken("single-token", "same-rt");
        var api = new FakeAuthorizationApi(response);
        var sut = CreateTokenService(storage, api);

        var results = await Task.WhenAll(
            sut.GetFreshAccessToken(),
            sut.GetFreshAccessToken(),
            sut.GetFreshAccessToken(),
            sut.GetFreshAccessToken(),
            sut.GetFreshAccessToken());

        Assert.All(results, r => Assert.Equal("single-token", r));
        Assert.Equal(1, api.RefreshTokenCallCount);
    }

    [Fact]
    public async Task GetRefreshedToken_ConcurrentCallsWithSameRefreshToken_RefreshCalledOnce_AllGetSameToken()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(NotExpiredAuth("any", "same-rt"));
        var response = FakeAuthorizationApi.ResponseWithAccessToken("single-token", "same-rt");
        var api = new FakeAuthorizationApi(async (_, _) =>
        {
            await Task.Delay(50);
            return response;
        });
        var sut = CreateTokenService(storage, api);

        var results = await Task.WhenAll(
            sut.GetRefreshedToken(),
            sut.GetRefreshedToken(),
            sut.GetRefreshedToken());

        Assert.All(results, r => Assert.Equal("single-token", r));
        Assert.Equal(1, api.RefreshTokenCallCount);
    }

    [Fact]
    public async Task GetFreshAccessToken_AfterFirstRefreshCompletes_SecondWaveWithSameKey_CallsRefreshAgain()
    {
        var storage = new FakeAuthenticationStorage();
        storage.SetCurrent(ExpiredAuth("old", "rt-one"));
        var api = FakeAuthorizationApi.WithSequentialResponses(
            FakeAuthorizationApi.ResponseWithAccessToken("first-token", "rt-one"),
            FakeAuthorizationApi.ResponseWithAccessToken("second-token", "rt-one"));
        var sut = CreateTokenService(storage, api);

        var first = await sut.GetFreshAccessToken();
        Assert.Equal("first-token", first);
        Assert.Equal(1, api.RefreshTokenCallCount);

        storage.SetCurrent(ExpiredAuth("first-token", "rt-one"));
        var second = await sut.GetFreshAccessToken();
        Assert.Equal(2, api.RefreshTokenCallCount);
        Assert.Equal("second-token", second);
    }
}
