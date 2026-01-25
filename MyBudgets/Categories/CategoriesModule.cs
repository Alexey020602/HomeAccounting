using MyBudgets.Categories.Data.DataBase;

namespace MyBudgets.Categories;

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