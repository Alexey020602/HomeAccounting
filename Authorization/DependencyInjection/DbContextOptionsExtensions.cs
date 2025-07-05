using Authorization.DataBase;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DependencyInjection;

public static class DbContextOptionsExtensions
{
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
        if (Queryable.FirstOrDefault(identityContext.Users, u => user.Id == u.Id) is not null) return;
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