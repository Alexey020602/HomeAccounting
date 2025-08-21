using MaybeResults;
using Refit;
using Shared.Utils;

namespace Shared.Blazor.RefitWithResult;

public static class ApiExceptionExtensions
{
    public static IEnumerator<NoneDetail> GetEnumerator(this ApiException apiException)
    {
        yield return new NoneDetail("StatusCode", apiException.StatusCode.ToString());
        if (apiException.Source is not null) {
            yield return new NoneDetail("Source", apiException.Source);
        }

        if (apiException.Content is not null)
        {
            yield return new NoneDetail("Content", apiException.Content);
        }
    }

    public static List<NoneDetail> GetDetails(this ApiException apiException)
    {
        var details = new List<NoneDetail>();
        foreach (var detail in apiException)
        {
            details.Add(detail);    
        }
        return details;
    }
}

[None]
public partial record ApiError
{
    public ApiError(ApiException apiException)
    {
        Message = apiException.Message;
        Details = apiException.GetDetails();
    }
}

public static class ApiResponseExtensions
{
    public static IMaybe ToMaybe(this ApiResponse<Unit> apiResponse)
    {
        if (apiResponse.IsSuccessful)
        {
            return Maybe.Create();
        }

        return apiResponse.Error.CreateApiError();
    }
    public static IMaybe<TResult> ToMaybe<TResult>(this ApiResponse<TResult> apiResponse)
    {
        if (apiResponse.IsSuccessful)
        {
            if (apiResponse.Content is not null)
            {
                return Maybe.Create(apiResponse.Content);
            }
            else
            {
                return new ApiError<TResult>("Successful response has no content");
            }
        }
        else
        {
            return apiResponse.Error.CreateApiError().Cast<TResult>();
        }
    }

    private static ApiError CreateApiError(this ApiException? apiException)
    {
        if (apiException is null)
        {
            return new ApiError("Failed response, but has no error");
        }
        else
        {
            return new ApiError(apiException);
        }
    }
}