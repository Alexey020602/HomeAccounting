using ServiceDefaults.ConfigOptions;

namespace HomeAccounting.Users.TokensCleanup;

internal sealed class TokensCleanupOptions: IConfigOptions
{
    public static string SectionName => "TokensCleanup";
    public int DayInterval { get; set; }
}