using Serilog.Events;

namespace HomeAccounting;

internal static class SerilogApplicationBuilderExtensions
{
    internal static LogEventLevel DefaultGetLevel(HttpContext httpContext, double elapsed, Exception? ex)
    {
        if (ex is not null || httpContext.Response.StatusCode >= 499) return LogEventLevel.Error;

        return httpContext.IsApiEndpoint()
            ? LogEventLevel.Information
            : LogEventLevel.Debug;
    }

    private static bool IsApiEndpoint(this HttpContext httpContext)
    {
        return httpContext.Request.Path.StartsWithSegments("/api");
    }
}