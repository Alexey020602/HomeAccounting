using MaybeResults;
using MudBlazor;

namespace BlazorConsolidated.Common.Components;

public static class MudExtensions
{
    public static Snackbar? ProcessError(this ISnackbar snackbar, INone error, Action<SnackbarOptions>? configure = null)
    {
        return snackbar.Add(error.Message, Severity.Error, configure);
    }
}