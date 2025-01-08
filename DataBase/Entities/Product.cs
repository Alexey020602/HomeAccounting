namespace DataBase.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } 
    public double Quantity { get; set; }
    public int Price { get; set; }
    public int Sum { get; set; }
    
}