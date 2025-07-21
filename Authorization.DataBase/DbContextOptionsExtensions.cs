using System.Linq.Expressions;
// using Authorization.Contracts;
using Authorization.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataBase;

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
        if (context is not IdentityUserContext<User, Guid> identityContext) return;

        var user = GetDefaultDevelopmentUser();
        if (identityContext.Users.FirstOrDefault(user.UserPredicate()) is not null) return;
        identityContext.Users.Add(user);
        identityContext.SaveChanges();
    }

    private static async Task SeedAsync(DbContext context, bool dbHasChanges, CancellationToken cancellationToken)
    {
        if (context is not IdentityUserContext<User, Guid> identityContext) return;
        
        var user = GetDefaultDevelopmentUser();
        if (await identityContext.Users.FirstOrDefaultAsync(user.UserPredicate(), cancellationToken) is not null) return;
        
        identityContext.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    private static Expression<Func<User, bool>> UserPredicate(this User developmentUser) => user => user.UserName == developmentUser.UserName;

    private static User GetDefaultDevelopmentUser()
    {
        var user = new User()
        {
            Id = Contracts.UserConstants.DefaultUserId,
            UserName = "chillexey",
            FullName = "Федоров Алексей",
            NormalizedUserName = "CHILLEXEY",
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, "7263A7263a+");
        return user;
    }
}