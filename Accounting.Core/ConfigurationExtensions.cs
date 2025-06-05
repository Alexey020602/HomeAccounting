using Microsoft.Extensions.Configuration;
namespace Accounting.Core;

public static class ConfigurationExtensions
{
    public static Settings GetSettings(this IConfiguration configuration) => 
        configuration.GetSection(Settings.AccountingSettingsSection).Get<Settings>() 
        ?? throw new Exception($"Missing {Settings.AccountingSettingsSection} in appsettings.json");
}