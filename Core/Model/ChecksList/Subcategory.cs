namespace Core.Model.ChecksList;

public class Subcategory
{
    public required int Id { get; set; }
    public required string? Name { get; set; }
    public required IReadOnlyList<Product> Products { get; set; }

    public int PennySum => Products.Sum(p => p.PennySum);

    public Sum Sum => new(PennySum);
}