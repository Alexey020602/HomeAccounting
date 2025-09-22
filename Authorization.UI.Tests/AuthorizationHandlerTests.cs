using System.Linq.Expressions;
using System.Net;
using System.Net.Http.Headers;
using Authorization.UI.Infrastructure;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using Shared.Blazor.Logout;

namespace Authorization.UI.Tests;

internal static class MockHttpMessageHandlerExtensions
{
    private static readonly object[] SendAsyncArguments =
    [
        ItExpr.IsAny<HttpRequestMessage>(),
        ItExpr.IsAny<CancellationToken>(),
    ];

    public static ISetup<HttpMessageHandler, Task<HttpResponseMessage>> SetupSendAsync(
        this Mock<HttpMessageHandler> mockHttpMessageHandler) =>
        mockHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                SendAsyncArguments
            );

    public static void VerifySendAsync(this Mock<HttpMessageHandler> mockHttpMessageHandler, Times times)
    {
        mockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            times,
            SendAsyncArguments
        );
    }
}

public class AuthorizationHandlerTests
{
    private const string TestUrl = "http://test.com";
    private const string TestAuthorizationScheme = "Bearer";
    private const string ValidToken = "access-token";
    private const string InvalidToken = "invalid-token";
    private static readonly HttpMethod DefaultRequestMethod = HttpMethod.Get;

    private static HttpRequestMessage RequestWithoutAuthorizationHeader => new(
        DefaultRequestMethod,
        TestUrl);

    private static HttpRequestMessage RequestWithEmptyAuthorizationHeader => new(
        DefaultRequestMethod,
        TestUrl
    )
    {
        Headers =
        {
            Authorization = new AuthenticationHeaderValue(TestAuthorizationScheme)
        }
    };

    private readonly Mock<ITokenService> tokenServiceMock = new();
    private readonly Mock<ILogoutService> logoutServiceMock = new();
    private readonly AuthorizationHandler authorizationHandler;
    private readonly Mock<HttpMessageHandler> httpMessageHandlerMock = new();
    private readonly HttpClient client;

    public AuthorizationHandlerTests()
    {
        authorizationHandler = new(
            tokenServiceMock.Object,
            logoutServiceMock.Object)
        {
            InnerHandler = httpMessageHandlerMock.Object
        };
        client = new HttpClient(authorizationHandler)
        {
            BaseAddress = new Uri(TestUrl)
        };
    }

    [Fact]
    public async Task SendAsync_NoAuthorizationHeader_SuccessRequest()
    {
        httpMessageHandlerMock.SetupSendAsync()
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
        var response = await client.SendAsync(RequestWithoutAuthorizationHeader);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        httpMessageHandlerMock.VerifySendAsync(Times.Once());
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
        
        httpMessageHandlerMock.SetupSendAsync()
            .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        httpMessageHandlerMock.VerifySendAsync(Times.Once());
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
        httpMessageHandlerMock.VerifySendAsync(Times.Never());
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
        var isSecondRequest = false;
        httpMessageHandlerMock.SetupSendAsync()
            .Returns((HttpRequestMessage requestMessage, CancellationToken _) =>
            {
                if (isSecondRequest)
                {
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
                }
                else
                {
                    isSecondRequest = true;
                    return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
            });

        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        httpMessageHandlerMock.VerifySendAsync(Times.Exactly(2));
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
        tokenServiceMock
            .Setup(storage => storage.GetRefreshedToken(It.IsAny<CancellationToken>()))
            .Throws<Exception>();
        httpMessageHandlerMock.SetupSendAsync()
            .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Unauthorized));

        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        httpMessageHandlerMock.VerifySendAsync(Times.Once());
        tokenServiceMock.Verify(storage => storage.GetFreshAccessToken(It.IsAny<CancellationToken>()), Times.Once);
        tokenServiceMock.Verify(api => api.GetRefreshedToken(It.IsAny<CancellationToken>()), Times.Once);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Once);
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
        public int RequestsCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            RequestsCount++;
            return Task.FromResult(
                IsRequestAuthorized(request.Headers
                    .Authorization) /*authorization || authorization is {Parameter: ValidToken}*/
                    ? new HttpResponseMessage(HttpStatusCode.OK)
                    : new HttpResponseMessage(HttpStatusCode.Unauthorized));
        }

        private static bool IsRequestAuthorized(AuthenticationHeaderValue? authenticationHeaderValue) =>
            authenticationHeaderValue is null or { Parameter: ValidToken };
    }
}