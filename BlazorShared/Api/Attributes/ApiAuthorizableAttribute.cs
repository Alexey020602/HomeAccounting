namespace BlazorShared.Api.Attributes;

[AttributeUsage(AttributeTargets.Interface)]
public class ApiAuthorizableAttribute(string basePath = ""): ApiAttribute(basePath)
{
    
}