using MaybeResults;
using Refit;

namespace Shared.Blazor.RefitWithResult;

[None]
public partial record ApiError
{
    public ApiError(ApiException apiException)
    {
        Message = apiException.Message;
        Details = apiException.GetDetails();
    }

    
}