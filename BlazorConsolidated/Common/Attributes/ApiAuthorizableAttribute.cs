namespace BlazorConsolidated.Common.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiAuthorizableAttribute(string basePath = ""): ApiAttribute(basePath)
{
    
}