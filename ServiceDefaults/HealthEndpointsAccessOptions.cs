using ServiceDefaults.ConfigOptions;

namespace ServiceDefaults;

/// <summary>
/// Defines how management health endpoints are exposed.
/// </summary>
public sealed class HealthEndpointAccessOptions: IConfigOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public static string SectionName => "HealthEndpoints";

    /// <summary>
    /// Gets the TCP port on which management health endpoints are expected to be served.
    /// </summary>
    public int ManagementPort { get; init; } = 8081;
}