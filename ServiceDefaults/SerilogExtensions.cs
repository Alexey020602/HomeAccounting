using Serilog;
using Serilog.Events;
using Serilog.Sinks.OpenTelemetry;
using Serilog.Sinks.SystemConsole.Themes;

namespace ServiceDefaults;

/// <summary>
/// Provides a shared Serilog baseline that every service in the solution can reuse.
/// Application-specific enrichers and overrides should be added on top in each Program.cs.
/// </summary>
public static class SerilogExtensions
{
    /// <summary>
    /// Configures common Serilog defaults: minimum levels, console sink, OpenTelemetry sink,
    /// and <c>FromLogContext</c> enrichment.
    /// </summary>
    public static LoggerConfiguration AddDefaultSerilog(this LoggerConfiguration configuration)
    {
        return configuration
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .WriteTo.Console(
                theme: ConsoleTheme.None,
                applyThemeToRedirectedOutput: false,
                outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
            .WriteTo.OpenTelemetry(includedData:
                IncludedData.MessageTemplateTextAttribute |
                IncludedData.SpanIdField |
                IncludedData.TraceIdField)
            .Enrich.FromLogContext();
    }
}
