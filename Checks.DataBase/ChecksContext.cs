using Checks.DataBase.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Checks.DataBase;

public class ChecksContext(DbContextOptions<ChecksContext> options)
    : DbContext(options)
{
    internal DbSet<Category> Categories { get; set; } = null!;
    internal DbSet<Product> Products { get; set; } = null!;
    internal DbSet<Subcategory> Subcategories { get; set; } = null!;
    internal DbSet<Check> Checks { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("Checks");
    }
}