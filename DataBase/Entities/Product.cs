namespace DataBase.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Quantity { get; set; }
    public int Price { get; set; }
    public int Sum { get; set; }
    public Category Category { get; set; } = null!;
}