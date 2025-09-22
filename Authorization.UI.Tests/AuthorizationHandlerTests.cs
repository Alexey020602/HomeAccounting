using System.Net;
using System.Net.Http.Headers;
using Authorization.Contracts;
using Authorization.UI.Dto;
using Authorization.UI.Infrastructure;
using Moq;
using Shared.Blazor.Logout;

namespace Authorization.UI.Tests;

public class AuthorizationHandlerTests
{
    private const string TestUrl = "http://test.com";
    private const string TestAuthorizationScheme = "Bearer";
    private const string ValidToken = "access-token";
    private const string ValidRefreshToken = "refresh-token";
    private const string InvalidToken = "invalid-token";
    private const string InvalidRefreshToken = "invalid-refresh-token";
    private static readonly HttpMethod DefaultRequestMethod = HttpMethod.Get;

    private static readonly HttpRequestMessage RequestWithoutAuthorizationHeader = new(
        DefaultRequestMethod,
        TestUrl);

    private static readonly HttpRequestMessage RequestWithEmptyAuthorizationHeader = new(
        DefaultRequestMethod,
        TestUrl
    )
    {
        Headers = { Authorization = new AuthenticationHeaderValue(TestAuthorizationScheme) }
    };

    private static readonly User DefaultUser = new User(Guid.CreateVersion7(), "username", "fullname");

    private static readonly Authentication DefaultAuthentication = new Authentication(
        ValidToken,
        ValidRefreshToken,
        DefaultUser,
        DateTime.UtcNow.AddHours(1)
    );

    private static readonly Authentication InvalidAuthentication = new(
        InvalidToken,
        ValidRefreshToken,
        DefaultUser,
        DateTime.UtcNow.AddHours(1));

    private readonly Mock<ITokenService> tokenServiceMock;
    private readonly Mock<ILogoutService> logoutServiceMock;
    private readonly AuthorizationHandler authorizationHandler;
    private readonly HttpMessageHandler innerHandler;
    private readonly HttpClient client;

    public AuthorizationHandlerTests()
    {
        tokenServiceMock = new();
        logoutServiceMock = new();
        innerHandler = new OtherMockHttpMessageHandler();
        authorizationHandler = new(
            tokenServiceMock.Object,
            logoutServiceMock.Object)
        {
            InnerHandler = innerHandler
        };
        client = new HttpClient(authorizationHandler)
        {
            BaseAddress = new Uri(TestUrl)
        };
    }

    [Fact]
    public async Task SendAsync_NoAuthorizationHeader_SuccessRequest()
    {
        var response = await client.SendAsync(RequestWithoutAuthorizationHeader);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        tokenServiceMock.Verify(storage => storage.GetFreshAccessToken(
                It.IsAny<CancellationToken>()),
            Times.Never);
        tokenServiceMock.Verify(storage => storage.GetRefreshedToken(
                It.IsAny<CancellationToken>()),
            Times.Never);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SendAsync_WithAuthorizationHeader_SuccessRequest()
    {
        tokenServiceMock.Setup(storage => storage
                .GetFreshAccessToken(It.IsAny<CancellationToken>()))
            .ReturnsAsync(ValidToken);

        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        tokenServiceMock.Verify(storage => storage.GetFreshAccessToken(It.IsAny<CancellationToken>()),
            Times.Once);
        tokenServiceMock.Verify(api => api.GetRefreshedToken(It.IsAny<CancellationToken>()), Times.Never);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SendAsync_WithAuthorizationHeader_NullToken_Logout()
    {
        tokenServiceMock.Setup(storage => storage
                .GetFreshAccessToken(It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(() => null);
        
        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        
        tokenServiceMock.Verify(storage => storage.GetFreshAccessToken(It.IsAny<CancellationToken>()), Times.Once);
        tokenServiceMock.Verify(api => api.GetRefreshedToken(It.IsAny<CancellationToken>()), Times.Never);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task SendAsync_WithInvalidAuthorizationHeader_SuccessRequest()
    {
        tokenServiceMock.Setup(storage => storage
                .GetFreshAccessToken(It.IsAny<CancellationToken>()))
            .ReturnsAsync(InvalidToken);

        tokenServiceMock.Setup(storage => storage
                .GetRefreshedToken(It.IsAny<CancellationToken>())
            )
            .ReturnsAsync(ValidToken);

        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        tokenServiceMock.Verify(storage => storage.GetFreshAccessToken(It.IsAny<CancellationToken>()), Times.Once);
        tokenServiceMock.Verify(api => api.GetRefreshedToken(It.IsAny<CancellationToken>()), Times.Once);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Never);
    }
    [Fact]
    public async Task SendAsync_WithInvalidAuthorizationHeader_ErrorOnRefresh()
    {
        tokenServiceMock
            .Setup(storage => storage.GetFreshAccessToken(It.IsAny<CancellationToken>()))
            .ReturnsAsync(InvalidToken);
        
        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);
        
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    // [Fact]
    // public async Task SendAsync_UnauthorizedResponse_RefreshToken_RepeatRequest()
    // {
    //     var authenticationResponse = new AuthorizationResponse(
    //         TestAuthorizationScheme, 
    //         DefaultUser,
    //         ValidToken,
    //         ValidRefreshToken,
    //         DateTime.UtcNow.AddHours(1)
    //         );
    // }

    private sealed class MockHttpMessageHandler(Func<int, HttpResponseMessage>? responseFactory = null)
        : HttpMessageHandler
    {
        private int requestsCount;

        public Func<int, HttpResponseMessage> ResponseFactory { get; set; } =
            responseFactory ?? ((_) => new HttpResponseMessage(HttpStatusCode.OK));

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            try
            {
                return Task.FromResult(ResponseFactory(requestsCount));
            }
            finally
            {
                requestsCount++;
            }
        }
    }

    private sealed class OtherMockHttpMessageHandler() : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken) =>
            Task.FromResult(
                IsRequestAuthorized(request.Headers
                    .Authorization) /*authorization || authorization is {Parameter: ValidToken}*/
                    ? new HttpResponseMessage(HttpStatusCode.OK)
                    : new HttpResponseMessage(HttpStatusCode.Unauthorized));

        private static bool IsRequestAuthorized(AuthenticationHeaderValue? authenticationHeaderValue) =>
            authenticationHeaderValue is null or { Parameter: ValidToken };
    }
}