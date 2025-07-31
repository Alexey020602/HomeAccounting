using MaybeResults;
using Microsoft.AspNetCore.Identity;

namespace Authorization.Core.Registration;

[None]
public partial record UserError
{
    public UserError(string message, IdentityResult result) 
    {
        if (result.Succeeded && !result.Errors.Any())
        {
            throw new InvalidOperationException("No errors in UserError creation");
        }

        Message = message;
        Details = result.ConvertToNoneDetails();
    }
}

static class IdentityErrorExtensions
{
    public static IReadOnlyCollection<NoneDetail> ConvertToNoneDetails(this IdentityResult result) =>
        result.Errors.Select(ConvertToNoneDetail).ToList();
    public static NoneDetail ConvertToNoneDetail(this IdentityError identityError) => new NoneDetail(identityError.Code, identityError.Description);
}

[None]
public partial record UserNotFoundError;