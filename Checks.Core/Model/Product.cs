namespace Checks.Core.Model;

public record class Product(int Id, string Name, double Quantity, int Price, int Sum, Subcategory Subcategory)
{
    
}