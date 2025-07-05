using LightResults;

namespace Authorization.Core.Registration;

public sealed class UserCreationError(string code, string description) : Error(description)
{
    public string Code { get; } = code;
}