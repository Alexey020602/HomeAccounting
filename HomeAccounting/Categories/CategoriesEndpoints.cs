using HomeAccounting.Categories.GetCategories;
using HomeAccounting.Categories.GetCategoriesTree;

namespace HomeAccounting.Categories;

internal static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var categoriesGroup = endpoints.MapGroup("categories").AllowAnonymous();
        
        categoriesGroup.MapGetCategories();
        categoriesGroup.MapGetCategoriesTree();
    }
}