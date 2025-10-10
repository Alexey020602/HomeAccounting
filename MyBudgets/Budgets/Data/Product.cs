namespace MyBudgets.Budgets.Data;

sealed class Product
{
    private const string DefaultName = "Unitialized";
    public int Id { get; set; }
    public string Name { get; set; } = DefaultName;
    public double Quantity { get; set; }
    public int Price { get; set; }
    public int Sum { get; set; }
}