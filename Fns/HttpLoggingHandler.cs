using Microsoft.Extensions.Logging;

namespace Fns;

public class HttpLoggingHandler(ILogger<HttpLoggingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var id = Guid.NewGuid();
        var httpResponseMessage = await base.SendAsync(request, cancellationToken);
        await LogRequest(request, id);
        await LogResponse(httpResponseMessage, id);
        return httpResponseMessage;
    }

    private async Task LogRequest(HttpRequestMessage request, Guid id)
    {
        logger.LogInformation("[{Id}] Request: {Request}", id, request);
        if (request.Content is null) return;
        var contentString = await request.Content.ReadAsStringAsync();
        logger.LogDebug("[{Id}] Request content: {RequestContent}", id, contentString);
    }

    private async Task LogResponse(HttpResponseMessage httpResponseMessage, Guid id)
    {
        logger.LogInformation("[{Id}] Response: {Response}", id, httpResponseMessage);
        var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
        logger.LogDebug("[{Id}] Response content: {ResponseContent}", id, contentString);
    }
}