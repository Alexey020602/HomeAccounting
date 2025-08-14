using MaybeResults;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Shared.Blazor.Components;

public static class MudExtensions
{
    public static void ProcessError(this ISnackbar snackbar, INone error, Action<SnackbarOptions>? configure = null)
    {
        snackbar.Add(error.Message, Severity.Error, configure);
    }
}