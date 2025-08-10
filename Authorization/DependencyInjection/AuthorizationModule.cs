using Authorization.Core;
using Authorization.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Shared.Infrastructure;

namespace Authorization.DependencyInjection;

public static class AuthorizationModule
{
    public static IServiceCollection AddAuthorization(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddDbContext<AuthorizationContext>(
            databaseServiceName,
            optionsAction: options =>
            {
                options
                    .SetUpAuthorizationForDevelopment();
            },
            npgsqlOptionsAction:options => options
                    .MigrationsHistoryTable(DbConstants.MigrationTableName, AuthorizationDbConstants.ShemaName)
            );
        builder.Services.AddIdentityCore<User>(_ =>
            {
                
            })
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

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddTransient<ITokenService, TokenService>();
        return builder.Services;
    }
}