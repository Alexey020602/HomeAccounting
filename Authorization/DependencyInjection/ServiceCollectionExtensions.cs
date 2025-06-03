using Authorization.Extensions;
using Checks.DataBase;
using Checks.DataBase.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<ApplicationContext>();
        
        services.AddAuthorization();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = configuration.CreateJwtTokenSettings().TokenValidationParameters;
        });

        services.AddTransient<ITokenService, TokenService>();
        return services;
    }
}