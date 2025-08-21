using System.Net;
using MaybeResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Shared.Utils.Results;
using FailureResults = Microsoft.AspNetCore.Http.HttpResults.Results<Microsoft.AspNetCore.Http.HttpResults.ForbidHttpResult, Microsoft.AspNetCore.Http.HttpResults.ValidationProblem, Microsoft.AspNetCore.Http.HttpResults.ProblemHttpResult>;
namespace Shared.Web;

public static class MaybeResultMapper
{
    public static Results<Ok<T>, FailureResults> MapToResult<T>(
        this IMaybe<T> maybe
    ) => maybe.MapToResult(onSuccess: TypedResults.Ok);
    public static Results<TResult, FailureResults> MapToResult<TResult, T>(
        this IMaybe<T> maybe, 
        Func<T, TResult> onSuccess
        ) where TResult : IResult 
        =>
        maybe.MapToResult(
            onSuccess: onSuccess,
            onFailure: error => error.MapToFailureResult()
        );

    public static Results<TSuccessResult, TFailureResult> MapToResult<TSuccessResult, TFailureResult, T>(
        this IMaybe<T> maybe, Func<T, TSuccessResult> onSuccess, Func<INone<T>, TFailureResult> onFailure) 
        where TSuccessResult: IResult 
        where TFailureResult: IResult 
        => maybe switch
    {
        Some<T> some => onSuccess(some.Value),
        INone<T> error => onFailure(error),
        _ => throw new InvalidOperationException($"Unknown IMaybe<{nameof(T)}> state"),
    };
    public static
        FailureResults
        MapToFailureResult(this INone error)
    {
        return error switch
        {
            IForbiddenError => TypedResults.Forbid(),
            INotFoundError notFoundError => notFoundError.MapToProblemResult(),
            _ => error.MapToValidationProblemResult(),
        };
    }
}
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