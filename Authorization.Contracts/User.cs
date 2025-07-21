using System.Text.Json.Serialization;

namespace Authorization.Contracts;

[method: JsonConstructor]
public record User(Guid Id, string UserName);