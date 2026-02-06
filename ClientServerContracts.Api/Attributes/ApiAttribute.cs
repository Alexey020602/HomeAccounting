namespace ClientServerContracts.Api.Attributes;

/// <summary>
/// Attribute for marking Refit API interfaces with base path.
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class ApiAttribute(string? basePath = null) : Attribute
{
    /// <summary>
    /// Base path for the API endpoint.
    /// </summary>
    public string? BasePath { get; } = basePath;
}
