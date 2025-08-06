using Authorization.Core;
using Authorization.DataBase;
using Authorization.UI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        builder.Services.AddIdentityCore<User>(_ =>
            {
                
            })
            .AddEntityFrameworkStores<AuthorizationContext>();
        
        builder.Services.AddAuthorization(options => options.AddPolicy(
            Authorization.UI.AuthorizationModule.UserbyidPolicyName,
            policy =>
            {
                policy.RequireAuthenticatedUser()
                    .AddRequirements(new UserRequirement());
            })
        );
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = builder.Configuration.CreateJwtTokenSettings().TokenValidationParameters;
        });
        builder.Services.AddSingleton<IAuthorizationHandler, UserAuthorizationHandler>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddTransient<ITokenService, TokenService>();
        return builder.Services;
    }
}