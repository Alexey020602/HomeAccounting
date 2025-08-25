using System.Net;
using MaybeResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Shared.Utils.Results;

namespace Shared.Web;

public static class MaybeResultMapper
{
    #region Async with ValueTask results mapping
    public static Task<Results<Ok, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync(this ValueTask<IMaybe> maybeTask) =>
        maybeTask.MapToResultAsync(TypedResults.Ok);
    public static Task<Results<Ok<T>, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync<T>(this ValueTask<IMaybe<T>> maybeTask) =>
        maybeTask.MapToResultAsync(TypedResults.Ok);
    public static Task<Results<TResult, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync<TResult>(
        this ValueTask<IMaybe> maybeTask,
        Func<TResult> onSuccess) where TResult : IResult
        => maybeTask.MapToResultAsync(onSuccess, error => error.MapToFailureResult());
    public static Task<Results<TResult, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync<TResult, T>(
        this ValueTask<IMaybe<T>> maybeTask,
        Func<T, TResult> onSuccess) where TResult : IResult
        => maybeTask.MapToResultAsync(onSuccess, error => error.MapToFailureResult());
    public static async Task<Results<TSuccessResult, TFailureResult>> MapToResultAsync<TSuccessResult, TFailureResult>(
        this ValueTask<IMaybe> maybeTask,
        Func<TSuccessResult> onSuccess,
        Func<INone, TFailureResult> onFailure
    )
        where TSuccessResult : IResult
        where TFailureResult : IResult
        => (await maybeTask).MapToResult(onSuccess, onFailure);
    public static async Task<Results<TSuccessResult, TFailureResult>> MapToResultAsync<TSuccessResult, TFailureResult,
        T>(
        this ValueTask<IMaybe<T>> maybeTask,
        Func<T, TSuccessResult> onSuccess,
        Func<INone<T>, TFailureResult> onFailure
    )
        where TSuccessResult : IResult
        where TFailureResult : IResult
        => (await maybeTask).MapToResult(onSuccess, onFailure);
    #endregion
    
    #region Async with Task results mapping
    public static Task<Results<Ok, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync(this Task<IMaybe> maybeTask) =>
        maybeTask.MapToResultAsync(TypedResults.Ok);
    public static Task<Results<Ok<T>, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync<T>(this Task<IMaybe<T>> maybeTask) =>
        maybeTask.MapToResultAsync(TypedResults.Ok);
    public static Task<Results<TResult, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync<TResult>(
        this Task<IMaybe> maybeTask,
        Func<TResult> onSuccess) where TResult : IResult
        => maybeTask.MapToResultAsync(onSuccess, error => error.MapToFailureResult());
    public static Task<Results<TResult, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>>> MapToResultAsync<TResult, T>(
        this Task<IMaybe<T>> maybeTask,
        Func<T, TResult> onSuccess) where TResult : IResult
        => maybeTask.MapToResultAsync(onSuccess, error => error.MapToFailureResult());
    public static async Task<Results<TSuccessResult, TFailureResult>> MapToResultAsync<TSuccessResult, TFailureResult,
        T>(
        this Task<IMaybe<T>> maybeTask,
        Func<T, TSuccessResult> onSuccess,
        Func<INone<T>, TFailureResult> onFailure
    )
        where TSuccessResult : IResult
        where TFailureResult : IResult
        => (await maybeTask).MapToResult(onSuccess, onFailure);
    public static async Task<Results<TSuccessResult, TFailureResult>> MapToResultAsync<TSuccessResult, TFailureResult>(
        this Task<IMaybe> maybeTask,
        Func<TSuccessResult> onSuccess,
        Func<INone, TFailureResult> onFailure
    )
        where TSuccessResult : IResult
        where TFailureResult : IResult
        => (await maybeTask).MapToResult(onSuccess, onFailure);
    #endregion
    
    #region Syncronious results mapping
    public static Results<Ok, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>> MapToResult(
        this IMaybe maybe
    ) => maybe.MapToResult(onSuccess: TypedResults.Ok);
    public static Results<Ok<T>, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>> MapToResult<T>(
        this IMaybe<T> maybe
    ) => maybe.MapToResult(onSuccess: TypedResults.Ok);

    
    public static Results<TResult, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>> MapToResult<TResult>(
        this IMaybe maybe,
        Func<TResult> onSuccess
    ) where TResult : IResult
        =>
            maybe.MapToResult(
                onSuccess: onSuccess,
                onFailure: error => error.MapToFailureResult()
            );
    public static Results<TResult, Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult>> MapToResult<TResult, T>(
        this IMaybe<T> maybe,
        Func<T, TResult> onSuccess
    ) where TResult : IResult
        =>
            maybe.MapToResult(
                onSuccess: onSuccess,
                onFailure: error => error.MapToFailureResult()
            );
    public static Results<TSuccessResult, TFailureResult> MapToResult<TSuccessResult, TFailureResult>(
        this IMaybe maybe, Func<TSuccessResult> onSuccess, Func<INone, TFailureResult> onFailure)
        where TSuccessResult : IResult
        where TFailureResult : IResult
        => maybe switch
        {
            Some => onSuccess(),
            INone error => onFailure(error),
            _ => throw new InvalidOperationException("Unknown IMaybe state"),
        };
    public static Results<TSuccessResult, TFailureResult> MapToResult<TSuccessResult, TFailureResult, T>(
        this IMaybe<T> maybe, Func<T, TSuccessResult> onSuccess, Func<INone<T>, TFailureResult> onFailure)
        where TSuccessResult : IResult
        where TFailureResult : IResult
        => maybe switch
        {
            Some<T> some => onSuccess(some.Value),
            INone<T> error => onFailure(error),
            _ => maybe.ToMaybe() is Some 
                ? throw new InvalidOperationException($"Unhandled successful state of result with type {maybe.GetType()}")
                : throw new InvalidOperationException($"Unknown IMaybe<{nameof(T)}> state"),
        };

    #endregion
    public static Results<ForbidHttpResult, ValidationProblem, ProblemHttpResult> MapToFailureResult(this INone error) => error switch
    {
        IForbiddenError => TypedResults.Forbid(),
        INotFoundError notFoundError => notFoundError.MapToProblemResult((int) HttpStatusCode.NotFound),
        _ => error.MapToValidationProblemResult(),
    };
}