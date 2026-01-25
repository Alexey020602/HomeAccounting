using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;
using HomeAccounting.Users.GetUsers;
using HomeAccounting.Users.Login;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HomeAccounting.Users;

static class UsersModule
{
    public static void AddUsers(this IHostApplicationBuilder builder, string databaseServiceName)
    {

        builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetRequiredSection(nameof(JwtTokenSettings)));
        
        builder.AddDatabase(databaseServiceName);

        builder.Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<UsersContext>();
        
        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = builder.Configuration
                                                        .GetRequiredSection(nameof(JwtTokenSettings))
                                                        .Get<JwtTokenSettings>()?.TokenValidationParameters ??
                                                    throw new InvalidOperationException(
                                                        "Missing JwtTokenSettings in appsettings.json");
            });
        
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        
        builder.Services.AddScoped<IUsersService, UsersService>();
    }
    
}