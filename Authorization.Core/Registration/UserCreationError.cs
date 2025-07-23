using LightResults;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Core.Registration;

public sealed class UserCreationError(string code, string description) : Error(description)
{
    public string Code { get; } = code;

    public UserCreationError(IdentityError identityError) : this(identityError.Code, identityError.Description)
    {
    }
    
    
}

public sealed class UserNotFoundError(string description): Error(description);