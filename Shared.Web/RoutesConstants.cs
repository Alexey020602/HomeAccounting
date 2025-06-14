using System.Diagnostics.CodeAnalysis;

namespace Shared.Web;

public static class RoutesConstants
{
    [StringSyntax("Route")] public const string ControllerRoute = "/api/[controller]";
}