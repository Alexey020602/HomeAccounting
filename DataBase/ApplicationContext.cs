using DataBase.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Subcategory> Subcategories { get; set; } = null!;
    public DbSet<Check> Checks { get; set; } = null!;
}