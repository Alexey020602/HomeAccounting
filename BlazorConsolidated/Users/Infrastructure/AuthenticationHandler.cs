using System.Net;
using System.Net.Http.Headers;
using BlazorConsolidated.Common.Logout;
using BlazorConsolidated.Users.Infrastructure.Abstractions;

namespace BlazorConsolidated.Users.Infrastructure;

public class AuthenticationHandler(
    ITokenService tokenService, ILogoutService logoutService)
    : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (request.Headers.Authorization is not {} auth) 
            return await base.SendAsync(request, cancellationToken);

        if (await tokenService.GetFreshAccessToken(cancellationToken) is not {} accessToken)
        {
            await logoutService.Logout(cancellationToken);
            return CreateUnauthorizedMessage(request);
        }

        request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, accessToken);
        var response = await base.SendAsync(request, cancellationToken);
        if (response is not { StatusCode: HttpStatusCode.Unauthorized})
            return response;

        try
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue(auth.Scheme, await tokenService.GetRefreshedToken(cancellationToken));
        }
        catch 
        {
            await logoutService.Logout(cancellationToken);
            return CreateUnauthorizedMessage(request);
        }
        finally
        {
            response.Dispose();
        }

        var secondResponse = await base.SendAsync(request, cancellationToken);
        if (secondResponse.StatusCode == HttpStatusCode.Unauthorized) 
            await logoutService.Logout(cancellationToken);
        return secondResponse;
    }

    private static HttpResponseMessage CreateUnauthorizedMessage(HttpRequestMessage? request)
    {
        return new HttpResponseMessage(HttpStatusCode.Unauthorized) { RequestMessage = request };
    }
}