using MaybeResults;
using MudBlazor;

namespace BlazorConsolidated.Common.Components;

public static class MudExtensions
{
    extension(ISnackbar snackbar)
    {
        public Snackbar? ProcessError(INone error, Action<SnackbarOptions>? configure = null)
        {
            return snackbar.Add(error.Message, Severity.Error, configure);
        }

        public Snackbar? ProcessException(Exception exception,
            Action<SnackbarOptions>? configure = null)
        {
            return snackbar.Add(exception.Message, Severity.Error, configure);
        }
    }
}