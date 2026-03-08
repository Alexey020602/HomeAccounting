using HomeAccounting.Users.Data;
using HomeAccounting.Users.Data.Database;
using HomeAccounting.Users.GetUsers;
using HomeAccounting.Users.Login;
using HomeAccounting.Users.TokensCleanup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HomeAccounting.Users;

static class UsersModule
{
    public static void AddUsers(this IHostApplicationBuilder builder, string databaseServiceName)
    {

        builder.Services.Configure<JwtTokenSettings>(builder.Configuration.GetRequiredSection(nameof(JwtTokenSettings)));
        
        builder.AddDatabase(databaseServiceName);

        builder.Services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-";
        });

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
        
        builder.Services.AddTokensCleanup();
        
        builder.Services.AddScoped<ITokenProvider, TokenProvider>();
        
        builder.Services.AddScoped<IUsersService, UsersService>();
    }
    
}