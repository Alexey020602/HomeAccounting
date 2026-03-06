using System.Linq.Expressions;

namespace HomeAccounting.Categories.Data;

internal record struct CategoryHierarchy(string Path = "/")
{
    public CategoryId[] CategoriesPath() => Path
        .Split('/', StringSplitOptions.RemoveEmptyEntries)
        .Select(int.Parse)
        .Select(id => new CategoryId(id))
        .ToArray();

    public CategoryHierarchy(CategoryId[] ids):this($"/{string.Join("/", ids.Select(x => x.Value.ToString()))}/")
    {
    }

    public static CategoryHierarchy operator +(CategoryHierarchy hierarchy, CategoryId childCategoryId) => 
        new CategoryHierarchy(
            hierarchy.Path + childCategoryId.Value + "/"
        );
    
    /// <summary>
    /// Expression, проверяющий, включена ли текущая категория в число наследников категории с `id`
    /// </summary>
    /// <param name="id">id родительской категории</param>
    /// <returns></returns>
    public static Expression<Func<Category, bool>> IsCategoryIncludedIn(CategoryId id)
    {
        var str = $"/{id.Value}/";

        return category => category.Hierarchy.Path.Contains(str);
    }
};