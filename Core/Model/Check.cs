namespace Core.Model;

public class Check
{
    public required Guid Id { get; init; }
    public required DateTime PurchaseDate { get; init; }
    public required DateTime AddedDate { get; init; }
    public required IReadOnlyList<Product> Products { get; init; }
}