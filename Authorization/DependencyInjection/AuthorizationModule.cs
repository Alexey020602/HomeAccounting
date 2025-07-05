using Authorization.Core;
using Authorization.DataBase;
using Authorization.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authorization.DependencyInjection;

public static class AuthorizationModule
{
    public static IServiceCollection AddAuthorization(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddNpgsqlDbContext<AuthorizationContext>(
            databaseServiceName,
            configureDbContextOptions: options => options.SetUpAuthorizationForDevelopment()
            );
        builder.Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AuthorizationContext>();
        
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = builder.Configuration.CreateJwtTokenSettings().TokenValidationParameters;
        });

        builder.Services.AddTransient<ITokenService, TokenService>();
        builder.Services.AddScoped<IAuthenticationManager, AuthenticationManager>();
        return builder.Services;
    }
}