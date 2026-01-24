using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using MyBudgets.Common.Model;

namespace MyBudgets.Budgets.Data;
internal record struct ProductId(Guid Value);
internal sealed record ProductInput(string Name, double Quantity, Money Price, Money Sum, CategoryId? CategoryId);
internal sealed class Product: Entity<ProductId>
{
    private const string DefaultName = "Unitialized";
    public string Name { get; private set; } = DefaultName;
    public double Quantity { get; private set; }
    public Money Price { get; private set; }
    public Money Sum { get; private set; }
    public CategoryId? CategoryId { get; private set; }
    private Product() { }

    public Product(string name, double quantity, Money price, Money sum, CategoryId? categoryId)
    {
        Name = name;
        Quantity = quantity;
        Price = price;
        Sum = sum;
        CategoryId = categoryId;
    }

    public void ChangeCategory(CategoryId categoryId)
    {
        CategoryId = categoryId;
    }
    
    public void DeleteCategory()
    {
        CategoryId = null;
    }
}