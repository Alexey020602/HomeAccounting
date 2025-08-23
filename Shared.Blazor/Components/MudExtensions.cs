using MaybeResults;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shared.Blazor.Components;

public static class MudExtensions
{
    public static Snackbar? ProcessError(this ISnackbar snackbar, INone error, Action<SnackbarOptions>? configure = null)
    {
        return snackbar.Add(error.Message, Severity.Error, configure);
    }
}