using MaybeResults;
using Refit;
using Shared.Utils;

namespace Shared.Blazor.RefitWithResult;

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
    public static async Task<IMaybe> ToMaybe(this Task<ApiResponse<Unit>> apiResponse) => (await apiResponse).ToMaybe();

    public static async Task<IMaybe<T>> ToMaybe<T>(this Task<ApiResponse<T>> apiResponse) =>
        (await apiResponse).ToMaybe();
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