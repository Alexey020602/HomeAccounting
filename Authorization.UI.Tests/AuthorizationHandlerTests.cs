using System.Net;
using System.Net.Http.Headers;
using Authorization.Contracts;
using Authorization.UI.Dto;
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
        Headers = { Authorization = new AuthenticationHeaderValue(TestAuthorizationScheme)}
    };

    private static readonly User DefaultUser = new User(Guid.CreateVersion7(), "username", "fullname");

    private static readonly Authentication DefaultAuthentication = new Authentication(
        ValidToken, 
        ValidRefreshToken,
        DefaultUser,
        DateTime.UtcNow.AddHours(1)
    );
    private static readonly Authentication InvalidAuthentication = new (
        InvalidToken, 
        ValidRefreshToken, 
        DefaultUser,  
        DateTime.UtcNow.AddHours(1));
    private readonly Mock<IAuthenticationStorage> authenticationStorageMock;
    private readonly Mock<IAuthorizationApi> authorizationApiMock;
    private readonly Mock<ILogoutService> logoutServiceMock;
    private readonly AuthorizationHandler authorizationHandler;
    private readonly HttpMessageHandler innerHandler;
    private readonly HttpClient client;
    
    public AuthorizationHandlerTests()
    {
        authenticationStorageMock = new();
        authorizationApiMock = new();
        logoutServiceMock = new();
        innerHandler = new OtherMockHttpMessageHandler();
        authorizationHandler = new (
            authenticationStorageMock.Object,
            authorizationApiMock.Object, 
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
        authenticationStorageMock.Verify(storage => storage.GetAuthorizationAsync(
            It.IsAny<CancellationToken>()), 
            Times.Never);
        authorizationApiMock.Verify(api => api.RefreshToken(It.IsAny<string>()), Times.Never);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task SendAsync_WithAuthorizationHeader_SuccessRequest()
    {
        authenticationStorageMock.Setup(storage => storage
                .GetAuthorizationAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(DefaultAuthentication);
        
        var response = await client.SendAsync(RequestWithEmptyAuthorizationHeader);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        authenticationStorageMock.Verify(storage => storage.GetAuthorizationAsync(It.IsAny<CancellationToken>()),
            Times.Once);
        authorizationApiMock.Verify(api => api.RefreshToken(It.IsAny<string>()), Times.Never);
        logoutServiceMock.Verify(service => service.Logout(It.IsAny<CancellationToken>()), Times.Never);
    }

    // [Fact]
    // public async Task SendAsync_WithInvalidAuthorizationHeader_ErrorOnRefresh()
    // {
    //     
    // }
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

    private sealed class MockHttpMessageHandler(Func<int, HttpResponseMessage>? responseFactory = null): HttpMessageHandler
    {
        private int requestsCount;
        public Func<int, HttpResponseMessage> ResponseFactory { get; set; } = responseFactory ?? ((_) => new HttpResponseMessage(HttpStatusCode.OK));
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
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
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
            Task.FromResult(IsRequestAuthorized(request.Headers.Authorization) /*authorization || authorization is {Parameter: ValidToken}*/ 
                ? new HttpResponseMessage(HttpStatusCode.OK) 
                : new HttpResponseMessage(HttpStatusCode.Unauthorized));

        private static bool IsRequestAuthorized(AuthenticationHeaderValue? authenticationHeaderValue) => 
            authenticationHeaderValue is null or {Parameter: ValidToken};
    }
}