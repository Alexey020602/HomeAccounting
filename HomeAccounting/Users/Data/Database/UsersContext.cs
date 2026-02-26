using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Users.Data.Database;

sealed class UsersContext(DbContextOptions<UsersContext> options): IdentityUserContext<User, UserId>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new RefreshTokenConfiguration());
        builder.HasDefaultSchema(AuthorizationDbConstants.ShemaName);
    }
}

public static class AuthorizationDbConstants
{
    public const string? ShemaName = "Identity";
}