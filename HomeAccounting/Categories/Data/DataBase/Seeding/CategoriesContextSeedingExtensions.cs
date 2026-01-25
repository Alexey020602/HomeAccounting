using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Categories.Data.DataBase.Seeding;

internal static class CategoriesContextSeedingExtensions
{
    extension(CategoriesContext categoriesContext)
    {
        internal void AddCategories()
        {
            foreach (var category in Category.GetCategoriesForSeeding())
            {
                if (categoriesContext.Categories.Any(c=> c.Id == category.Id))  continue;
                categoriesContext.Categories.Add(category);
            }
        }

        internal async Task AddCategoriesAsync(CancellationToken cancellationToken)
        {
            foreach (var category in Category.GetCategoriesForSeeding())
            {
                if(await categoriesContext.Categories.AnyAsync(c=>c.Id == category.Id, cancellationToken)) continue;
                categoriesContext.Categories.Add(category);
            }
        }
    }
}