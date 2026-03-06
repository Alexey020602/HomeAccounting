using HomeAccounting.Budgets.Data.Database;
using HomeAccounting.Categories.Data.DataBase.Configurations;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.Data.DataBase;

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

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);

        configurationBuilder.Properties<CategoryHierarchy>()
            .HaveConversion<HierarchyConverter>();
    }
}