using Microsoft.AspNetCore.Components;

namespace BlazorConsolidated;

public static class NavigationManagerExtensions
{
    public static string GetCurrentPath(this NavigationManager navigationManager) =>
        navigationManager.ToBaseRelativePath(navigationManager.Uri);
}