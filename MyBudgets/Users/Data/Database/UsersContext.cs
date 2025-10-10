using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyBudgets.Users.Data.Database;

sealed class UsersContext(DbContextOptions<UsersContext> options): IdentityUserContext<User, UserId>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfiguration());
        builder.HasDefaultSchema(AuthorizationDbConstants.ShemaName);
    }
}

public static class AuthorizationDbConstants
{
    public const string? ShemaName = "Identity";
}