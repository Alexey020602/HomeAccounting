namespace BlazorConsolidated.Common.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiAttribute(string? basePath = null): Attribute
{
    public string? BasePath { get; } = basePath;
}