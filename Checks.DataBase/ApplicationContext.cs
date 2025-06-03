using Checks.DataBase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Checks.DataBase;

public class ApplicationContext(DbContextOptions<ApplicationContext> options)
    : IdentityUserContext<User, string>(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Subcategory> Subcategories { get; set; } = null!;
    public DbSet<Check> Checks { get; set; } = null!;

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     // base.OnConfiguring(optionsBuilder);
    //     optionsBuilder
    //         .UseAsyncSeeding(async (context, _, cancellationToken) =>
    //         {
    //             var defaultUser = await context.Set<User>().FirstOrDefaultAsync(
    //                 user => user.Id == Guid.Empty.ToString(),
    //                 cancellationToken);
    //
    //             if (defaultUser is not null) return;
    //             var user = new User()
    //             {
    //                 Id = Guid.Empty.ToString(),
    //                 UserName = "Пользователь не выбран",
    //             };
    //             await context.Set<User>().AddAsync(user, cancellationToken);
    //             await context.SaveChangesAsync(cancellationToken);
    //         });
    // }
}