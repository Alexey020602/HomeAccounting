namespace Shared.Utils;

public static class BoolExtensions
{
    public static void Toggle(this ref bool b) => b = !b;
}