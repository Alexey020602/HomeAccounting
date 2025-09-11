using MaybeResults;
using Refit;

namespace Shared.Blazor.RefitWithResult;

static class ApiExceptionExtensions
{
    public static async Task<ApiError> CreateFromApiError(this ApiException apiException)
    {
        if (await apiException.GetContentAsAsync<ProblemDetails>() is { } details)
        {
            return CreateApiErrorFromProblemDetails(details, apiException.Message);
        }
        return new ApiError(apiException.Message);
    }

    private static ApiError CreateApiErrorFromProblemDetails(ProblemDetails details, string defaultMessage)
    {
        return new ApiError(
            details.Title ?? defaultMessage, 
            CreateNoneDetailsFromProblemDetails(details));
    }

    private static List<NoneDetail> CreateNoneDetailsFromProblemDetails(ProblemDetails problemDetails)
    {
        List<NoneDetail> noneDetails = [];
        foreach (var detail in problemDetails)
        {
            noneDetails.Add(detail);
        }
        return noneDetails;
    }
    private static IEnumerator<NoneDetail> GetEnumerator(this ProblemDetails problemDetails)
    {
        if (problemDetails.Detail is not null)
        {
            yield return new NoneDetail("Description", problemDetails.Detail);
        }

        if (problemDetails.Type is not null)
        {
            yield return new NoneDetail("Type", problemDetails.Type);
        }

        if (problemDetails.Instance is not null)
        {
            yield return new NoneDetail("Instance", problemDetails.Instance);
        }

        foreach (var error in problemDetails.Errors)
        {
            yield return new NoneDetail(error.Key, string.Join('\n', error.Value));
        }

        foreach (var extension in problemDetails.Extensions)
        {
            if (extension.Value.ToString() is { } value)
            {
                yield return new NoneDetail(extension.Key, value);
            }
        }
    }
    
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