using Authorization.Extensions;
using Checks.DataBase;
using Checks.DataBase.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Authorization.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<AuthorizationContext>();
        
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