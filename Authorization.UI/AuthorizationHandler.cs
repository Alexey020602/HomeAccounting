using System.Net;
using System.Net.Http.Headers;
using Authorization.Contracts;
using Authorization.UI.Dto;
using Refit;
using Shared.Blazor;
using Shared.Blazor.Logout;

namespace Authorization.UI;

public static class AuthorizationResponseExtensions
{
    public static Authentication ConvertToAuthentication(this AuthorizationResponse response)
    {
        return new Authentication(
            response.AccessToken,
            response.RefreshToken,
            response.User,
            response.ExpiresAt
        );
    }
}

public class AuthorizationHandler(
    IAuthenticationStorage authenticationStorage,
    IAuthorizationApi authorizationApi,
    ILogoutService logoutService)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var auth = request.Headers.Authorization;
        if (auth is null) return await base.SendAsync(request, cancellationToken);

        var accessToken = await authenticationStorage.GetAuthorizationAsync(cancellationToken);

        if (accessToken is null)
        {
            await logoutService.Logout();
            return CreateUnauthorizedMessage(request);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, accessToken.AccessToken);
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        try
        {
            var authorizationResponse = await authorizationApi.RefreshToken(accessToken.RefreshToken);
            await authenticationStorage.SetAuthorizationAsync(authorizationResponse.ConvertToAuthentication(),
                cancellationToken);

            request.Headers.Authorization =
                new AuthenticationHeaderValue(auth.Scheme, authorizationResponse.AccessToken);
        }
        catch (Exception)
        {
            await logoutService.Logout();
            return CreateUnauthorizedMessage(request);
        }
        finally
        {
            response.Dispose();
        }

        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (ApiException apiException)
        {
            if (apiException.StatusCode == HttpStatusCode.Unauthorized)
            {
                await logoutService.Logout();
            }

            throw;
        }
    }

    private static HttpResponseMessage CreateUnauthorizedMessage(HttpRequestMessage? request)
    {
        return new HttpResponseMessage(HttpStatusCode.Unauthorized) { RequestMessage = request };
    }
}