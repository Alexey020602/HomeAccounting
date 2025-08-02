using MaybeResults;

namespace Shared.Utils.Results;

public static class ExceptionHandlingExtensions
{
}

public static class ExceptionHandler
{
    public static IMaybe Try(this Action action) => Try(action, HandleException);

    public static IMaybe Try(this Action action, Func<Exception, IMaybe> exceptionHandler)
    {
        try
        {
            action();
            return Maybe.Create();
        }
        catch (Exception ex)
        {
            return exceptionHandler(ex);
        }
    }

    public static IMaybe<T> Try<T>(this Func<T> func, Func<Exception, IMaybe<T>> exceptionHandler)
    {
        try
        {
            return Maybe.Create(func());
        }
        catch (Exception ex)
        {
            return exceptionHandler(ex);
        }
    }
    
    public static IMaybe<T> Try<T>(this Func<T> func) => Try(func, HandleException<T>);

    public static async Task<IMaybe> TryAsync(this Task task, Func<Exception, IMaybe> exceptionHandler)
    {
        try
        {
            await task;
            return Maybe.Create();
        }
        catch (Exception ex)
        {
            return exceptionHandler(ex);
        }
    }

    public static Task<IMaybe> TryAsync(this Task task) => TryAsync(task, HandleException);

    public static async Task<IMaybe<T>> TryAsync<T>(this Task<T> task, Func<Exception, IMaybe<T>> exceptionHandler)
    {
        try
        {
            return Maybe.Create(await task);
        }
        catch (Exception  e)
        {
            return exceptionHandler(e);
        }
    }
    public static Task<IMaybe<T>> TryAsync<T>(this Task<T> task) => TryAsync(task, HandleException<T>);
    
    private static ExceptionError HandleException(Exception ex) => new ExceptionError(ex);

    private static ExceptionError<TResult> HandleException<TResult>(Exception ex) => new ExceptionError<TResult>(ex);
        // new ExceptionError<TResult>(
        // ex.ErrorMessage(),
        // ex.GetNoneDetails());
}

// public partial record ExceptionError<TResult>(string Message, Exception Exception);
// [None] public partial record ExceptionError<TResult>(string Message, Exception Exception);