using Authorization.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authorization.DataBase;

public class AuthorizationContext(DbContextOptions<AuthorizationContext> options): IdentityUserContext<User, string>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfiguration());
        builder.HasDefaultSchema("Identity");
    }
} 