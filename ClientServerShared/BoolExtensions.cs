namespace ClientServerShared;

public static class BoolExtensions
{
    public static void Toggle(this ref bool b) => b = !b;
}