using Authorization.Models;
using Microsoft.Extensions.Configuration;

namespace Authorization.Extensions;

public static class ConfigurationExtensions
{
    public static JwtTokenSettings CreateJwtTokenSettings(this IConfiguration configuration) => 
        configuration.GetSection(JwtTokenSettings.SectionName).Get<JwtTokenSettings>() ?? throw new Exception("Missing JwtTokenSettings in app.config");
}