using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace BlazorConsolidated.Common;

internal sealed class WasmStreamingRequestHandler: DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.SetBrowserRequestStreamingEnabled(true);
        request.SetBrowserResponseStreamingEnabled(true);
        return base.SendAsync(request, cancellationToken);
    }
}