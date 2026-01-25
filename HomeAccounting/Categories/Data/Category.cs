using HomeAccounting.Common.Model;

namespace HomeAccounting.Categories.Data;
internal record struct CategoryId(int Value);
internal sealed partial class Category: Entity<CategoryId>
{
    public string Name { get; private set; }
    public CategoryId? ParentCategoryId { get; private set; }

    private Category()
    {
        Name = string.Empty;
    }
    
    public Category(string name, CategoryId? parentCategoryId)
    {
        Name = name;
        ParentCategoryId = parentCategoryId;
    }
}