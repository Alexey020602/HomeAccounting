using Microsoft.EntityFrameworkCore;

namespace DataBase.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public double Quantity { get; set; }
    public int Price { get; set; }
    public int Sum { get; set; }
    public Subcategory Subcategory { get; set; } = null!;

    public override string ToString() => $"{Name} Категория {Subcategory}";
}