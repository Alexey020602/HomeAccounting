using MaybeResults;
using Refit;

namespace BlazorConsolidated.Common.RefitWithResult;

public static class ApiResponseErrorHandler
{
    public static async Task<IMaybe> ToMaybe(this Task task)
    {
        try
        {
            await task;
            return Maybe.Create();
        }
        catch (ApiException apiException)
        {
            return await apiException.CreateFromApiError();
        }
    }

    public static async Task<IMaybe<T>> ToMaybe<T>(this Task<T> task)
    {
        try
        {
            return Maybe.Create(await task);
        }
        catch (ApiException apiException)
        {
            return (await apiException.CreateFromApiError()).Cast<T>();
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