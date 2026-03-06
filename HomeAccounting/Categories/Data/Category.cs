using System.Linq.Expressions;
using HomeAccounting.Common.Model;

namespace HomeAccounting.Categories.Data;

internal sealed partial class Category: Entity<CategoryId>
{
    public string Name { get; private set; }
    public CategoryId? ParentCategoryId { get; private set; }
    public Category? ParentCategory { get; private set; }
    private List<Category> children = [];
    public IReadOnlyList<Category> Children => children;
    public CategoryHierarchy Hierarchy;
    private Category()
    {
        Name = string.Empty;
    }
    
    public Category(string name, Category? parentCategory)
    {
        Name = name;
        ParentCategoryId = parentCategory?.Id;
        if (parentCategory is not null)
        {
            Hierarchy =  parentCategory.Hierarchy + parentCategory.Id;
        }
        else
        {
            Hierarchy = new CategoryHierarchy();
        }
    }
}