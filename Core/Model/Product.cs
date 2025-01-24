namespace Core.Model;

public class Product
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required double Quantity { get; init; }
    public required int Sum { get; init; }
    public required Subcategory Subcategory { get; init; }
}