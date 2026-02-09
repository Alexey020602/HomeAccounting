namespace ClientServerContracts.Api.Attributes;

/// <summary>
/// Attribute for marking Refit API interfaces that require authorization with base path.
/// </summary>
[AttributeUsage(AttributeTargets.Interface)]
public class ApiAuthorizableAttribute(string basePath = "") : ApiAttribute(basePath)
{
}
