using Authorization.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authorization.DependencyInjection;

public static class AuthorizationModule
{
    public static IServiceCollection AddAuthorization(this IHostApplicationBuilder builder, string databaseServiceName)
    {
        builder.AddNpgsqlDbContext<AuthorizationContext>(databaseServiceName);
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
        return builder.Services;
    }

    public static DbContextOptionsBuilder SetUpAuthorization(this DbContextOptionsBuilder options) => 
        options;

    public static DbContextOptionsBuilder SetUpAuthorizationForDevelopment(this DbContextOptionsBuilder options) =>
        options.SetUpAuthorization()
            .UseSeeding(Seed)
            .UseAsyncSeeding(SeedAsync);

    private static void Seed(DbContext context, bool dbHasChanges)
    {
        if (context is not IdentityUserContext<User, string> identityContext) return;

        var user = GetDefaultDevelopmentUser();
        if (identityContext.Users.FirstOrDefault(u => user.Id == u.Id) is not null) return;
        identityContext.Users.Add(user);
        identityContext.SaveChanges();
    }

    private static async Task SeedAsync(DbContext context, bool dbHasChanges, CancellationToken cancellationToken)
    {
        if (context is not IdentityUserContext<User, string> identityContext) return;
        
        var user = GetDefaultDevelopmentUser();
        if (await identityContext.Users.FirstOrDefaultAsync(u => user.Id == u.Id, cancellationToken) is not null) return;
        
        identityContext.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static User GetDefaultDevelopmentUser()
    {
        var user = new User()
        {
            Id = "chillexey",
            UserName = "Alexey",
            NormalizedUserName = "ALEXEY",
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "7263A7263a+");
        return user;
    }
}