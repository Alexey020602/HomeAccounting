namespace Shared.Blazor.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiAuthorizableAttribute(string basePath = ""): ApiAttribute(basePath)
{
    
}