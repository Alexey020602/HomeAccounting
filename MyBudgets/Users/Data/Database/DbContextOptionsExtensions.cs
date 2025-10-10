using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace MyBudgets.Users.Data.Database;

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
        if (context is not UsersContext identityContext) return;

        var user = GetDefaultDevelopmentUser();
        if (identityContext.Users.FirstOrDefault(user.UserPredicate()) is not null) return;
        identityContext.Users.Add(user);
        identityContext.SaveChanges();
    }

    private static async Task SeedAsync(DbContext context, bool dbHasChanges, CancellationToken cancellationToken)
    {
        if (context is not UsersContext identityContext) return;
        List<User> users =
        [
            GetDefaultDevelopmentUser(),
            GetSecondDevelopmentUser()
        ];

        foreach (var user in users)
        {
            await identityContext.AddUser(user, cancellationToken);
        }
        
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task AddUser(this UsersContext identityContext, User user, CancellationToken cancellationToken)
    {
        if (await identityContext.Users.FirstOrDefaultAsync(user.UserPredicate(), cancellationToken) is not null) return;
        identityContext.Users.Add(user);
    }
    
    private static Expression<Func<User, bool>> UserPredicate(this User developmentUser) => user => user.Id == developmentUser.Id;

    private static User GetDefaultDevelopmentUser()
    {
        return User.CreateForSeeding(UserConstants.DefaultUserId, "chillexey", "Федоров Алексей", "7263A7263a+");
    }

    private static User GetSecondDevelopmentUser()
    {
        return User.CreateForSeeding(UserConstants.SecondUserId, "alexflex", "Федоров Александр", "5605A5605a+");
    }
}