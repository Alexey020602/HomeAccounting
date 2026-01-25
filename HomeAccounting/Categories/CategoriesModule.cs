using HomeAccounting.Categories.Data.DataBase;

namespace HomeAccounting.Categories;

internal static class CategoriesModule
{
    extension(IHostApplicationBuilder builder)
    {
        public void AddCategories(string databaseServiceName)
        {
            builder.AddDatabase(databaseServiceName);
        }
    }
}