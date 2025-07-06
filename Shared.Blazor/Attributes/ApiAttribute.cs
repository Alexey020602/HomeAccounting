namespace Shared.Blazor.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiAttribute(string basePath = ""): Attribute
{
    public string BasePath { get; } = basePath;
}