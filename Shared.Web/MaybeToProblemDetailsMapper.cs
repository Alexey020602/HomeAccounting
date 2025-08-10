using System.Net;
using MaybeResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Shared.Web;

public static class MaybeToProblemDetailsMapper
{
    public static ProblemHttpResult MapToProblemResult(
        this INone error,
        int statusCode = (int)HttpStatusCode.BadRequest
        ) =>
        TypedResults.Problem(
            detail: error.Message,
            statusCode: statusCode
            );

    public static ValidationProblem MapToValidationProblemResult(this INone error) =>
        TypedResults.ValidationProblem(
            errors: error.GetErrorGroups(),
            detail: error.Message);
    
    public static ProblemDetails MapToProblemDetails(this INone error, int statusCode = (int)HttpStatusCode.BadRequest)
    {
        return new ProblemDetails()
        {
            Status = statusCode,
            Detail = error.Message,
            Extensions = error.Details.GetErrorDetails()
                // .Select(pair => (pair.Key, (object?) pair.Value))
                .ToDictionary(
                    pair => pair.Key,
                    object? (pair) => pair.Value
                )
        };
    }

    public static HttpValidationProblemDetails MapToValidationProblemDetails(this INone error,
        int statusCode = (int)HttpStatusCode.BadRequest)
    {
        return new HttpValidationProblemDetails()
        {
            Detail = error.Message,
            Status = statusCode,
            Errors = error.Details.GetErrorDetails()
        };
    }
    private static IEnumerable<KeyValuePair<string, string[]>> GetErrorGroups(this INone error) => error.Details
        .GroupBy(
        keySelector: noneDetail => noneDetail.Code,
        elementSelector: noneDetail => noneDetail.Description
    )
        .Select(group => new KeyValuePair<string, string[]>(group.Key, group.ToArray()));
    private static Dictionary<string, string[]> GetErrorDetails(this IReadOnlyCollection<NoneDetail> details) =>
        details
            .GroupBy(
                keySelector: noneDetail => noneDetail.Code,
                elementSelector: noneDetail => noneDetail.Description
            )
            .ToDictionary(
                group => group.Key,
                group => group.ToArray()
            );
}