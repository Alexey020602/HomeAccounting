namespace MyBudgets.Budgets.Data;
internal record struct CategoryId(int Value);
internal sealed class Category
{
    public CategoryId Id { get; private set; }
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