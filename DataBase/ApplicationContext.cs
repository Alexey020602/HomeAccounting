using DataBase.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : IdentityUserContext<IdentityUser<Guid>, Guid>(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Subcategory> Subcategories { get; set; } = null!;
    public DbSet<Check> Checks { get; set; } = null!;
}