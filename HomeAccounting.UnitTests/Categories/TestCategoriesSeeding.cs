using HomeAccounting.Categories.Data;

namespace HomeAccounting.UnitTests.Categories;

public class TestCategoriesSeeding
{
    [Fact]
    public void GetCategoriesForSeeding_ShouldGenerateUniqueIds()
    {
        var categories = Category.GetCategoriesForSeeding();
        
        var ids = categories.Select(category => category.Id.Value).ToList();
        var uniqueIds = ids.Distinct().ToList();
        
        Assert.Equal(ids.Count, uniqueIds.Count);

        var expectedIds = Enumerable.Range(1, categories.Count).ToList();
        Assert.Equal(expectedIds, ids.OrderBy(id => id).ToList());
    }

    [Fact]
    public void GetCategoriesForSeeding_ShouldSetCorrectParentCategoryIds()
    {
        // Arrange & Act
        var categories = Category.GetCategoriesForSeeding();
        var categoriesById = categories.ToDictionary(c => c.Id);

        // Assert - корневые категории имеют ParentCategoryId = null
        var rootCategories = categories.Where(c => c.ParentCategoryId == null).ToList();
        Assert.NotEmpty(rootCategories);

        // Assert - дочерние категории имеют правильный ParentCategoryId
        foreach (var category in categories)
        {
            if (category.ParentCategoryId.HasValue)
            {
                var parentId = category.ParentCategoryId.Value;
                Assert.True(categoriesById.ContainsKey(parentId), 
                    $"Категория '{category.Name}' (ID: {category.Id.Value}) ссылается на несуществующего родителя (ID: {parentId.Value})");
                
                var parent = categoriesById[parentId];
                Assert.NotNull(parent);
            }
        }
    }

    [Fact]
    public void GetCategoriesForSeeding_ShouldHaveCorrectHierarchy()
    {
        // Arrange & Act
        var categories = Category.GetCategoriesForSeeding();
        var categoriesById = categories.ToDictionary(c => c.Id);

        // Assert - проверка, что нет циклических ссылок
        foreach (var category in categories)
        {
            if (category.ParentCategoryId.HasValue)
            {
                var visited = new HashSet<CategoryId>();
                var current = category.ParentCategoryId;
                
                while (current.HasValue && !visited.Contains(current.Value))
                {
                    visited.Add(current.Value);
                    if (categoriesById.TryGetValue(current.Value, out var parent))
                    {
                        current = parent.ParentCategoryId;
                    }
                    else
                    {
                        break;
                    }
                }
                
                Assert.DoesNotContain(category.Id, visited);
            }
        }
    }
}