using System.Net;
using System.Net.Http.Headers;
using BlazorShared.Api;
using BlazorShared.Authorization.Dto;
using Core.Authorization;

namespace BlazorShared.Authorization;

public static class AuthorizationResponseExtensions
{
    public static Authentication ConvertToAuthentication(this AuthorizationResponse response) => new(response.AccessToken, response.RefreshToken, response.Login);
}
public class AuthorizationHandler(IAuthenticationStorage authenticationStorage, IAuthorizationApi authorizationApi): DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var auth = request.Headers.Authorization;
        if (auth is null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var accessToken = await authenticationStorage.GetAuthorizationAsync(cancellationToken);
        
        if (accessToken is null) return new HttpResponseMessage(HttpStatusCode.Unauthorized) { RequestMessage = request };
        
        request.Headers.Authorization =  new AuthenticationHeaderValue(auth.Scheme, accessToken.AccessToken);
        var response = await base.SendAsync(request, cancellationToken);
        if (response.StatusCode != HttpStatusCode.Unauthorized)
            return response;

        var authorizationResponse = await authorizationApi.RefreshToken(accessToken.RefreshToken);
        await authenticationStorage.SetAuthorizationAsync(authorizationResponse.ConvertToAuthentication(),
            cancellationToken);
        
        request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, authorizationResponse.AccessToken);
        
        response.Dispose();
        
        return await base.SendAsync(request, cancellationToken);
    }
    
}