using System.Text.Json.Serialization;

namespace Authorization.Contracts;

[method: JsonConstructor]
public record UpdatedUserDto(string UserName, string FullName);