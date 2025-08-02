using Microsoft.Extensions.Configuration;

namespace Authorization.Core;

public static class ConfigurationExtensions
{
    public static JwtTokenSettings CreateJwtTokenSettings(this IConfiguration configuration) => 
        configuration.GetSection(JwtTokenSettings.SectionName).Get<JwtTokenSettings>() ?? throw new TokenSettingsException("Missing JwtTokenSettings in appsettings.json");
}