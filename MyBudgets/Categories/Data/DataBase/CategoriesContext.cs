using Microsoft.EntityFrameworkCore;
using MyBudgets.Categories.Data.DataBase.Configurations;

namespace MyBudgets.Categories.Data.DataBase;

internal sealed class CategoriesContext(DbContextOptions<CategoriesContext> options): DbContext(options)
{
    public const string Schema = "categories";
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.HasDefaultSchema(Schema);
        
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}