using MaybeResults;

namespace Shared.Utils.Results;

public interface IMyError: INone;
[None]
public partial record ExceptionError: IMyError
{
    public ExceptionError(Exception exception)
    {
        Message = exception.ErrorMessage();
        Details = exception.GetNoneDetails();
    }
}

public partial record ExceptionError<T>: IMyError
{
    public ExceptionError(Exception exception)
    {
        Message = exception.ErrorMessage();
        Details = exception.GetNoneDetails();
    }
}

public static class ExceptionForErrorExtensions
{
    internal static string ErrorMessage(this Exception ex) => ex.Message; //$"{ex.GetType().Name}: {ex.Message}";

    internal static IReadOnlyCollection<NoneDetail> GetNoneDetails(this Exception ex)
    {
        var details = new List<NoneDetail>(5)
        {
            new("Message", ex.Message),
            new("ExceptionType", ex.GetType().Name),
        };
        
        if (ex.StackTrace != null)
        {
            details.Add(new NoneDetail("StackTrace", ex.StackTrace));
        }
        
        if (ex.Source != null) 
        {
            details.Add(new NoneDetail("Source", ex.Source));
        }
        
        if (ex.HelpLink != null) 
        {
            details.Add(new NoneDetail("HelpLink", ex.HelpLink));
        }
        
        return details;
    }
}