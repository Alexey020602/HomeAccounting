namespace Receipts.DataBase.Entities;

class Product
{
    private const string DefaultName = "Unitialized";
    public int Id { get; set; }
    public string Name { get; set; } = DefaultName;
    public double Quantity { get; set; }
    public int Price { get; set; }
    public int Sum { get; set; }
    public Subcategory Subcategory { get; set; } = Subcategory.Default;

    public override string ToString()
    {
        return $"{Name} Категория {Subcategory}";
    }
}