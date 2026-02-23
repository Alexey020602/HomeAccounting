using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.Data.DataBase.Seeding;

internal static class DbContextOptionsExtensions
{
    extension(DbContextOptionsBuilder optionsBuilder)
    {
        public DbContextOptionsBuilder AddCategoriesSeeding() => optionsBuilder.UseSeeding(Seed).UseAsyncSeeding(SeedAsync);

        private static void Seed(DbContext context, bool dbHasChanged)
        {
            if (context is not CategoriesContext categoriesContext) return;
            
            categoriesContext.AddCategories();
            categoriesContext.SaveChanges();
        }

        private static async Task SeedAsync(DbContext context, bool dbHasChanged, CancellationToken cancellationToken)
        {
            if (context is not CategoriesContext categoriesContext) return;

            await categoriesContext.AddCategoriesAsync(cancellationToken);
            
            await categoriesContext.SaveChangesAsync(cancellationToken);
        }
    }
}