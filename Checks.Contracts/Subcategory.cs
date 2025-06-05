using Shared.Model;

namespace Checks.Contracts;

public class Subcategory
{
    public required int Id { get; set; }
    public required string? Name { get; set; }
    public required IReadOnlyList<Product> Products { get; set; }

    public int PennySum => Products.Sum(p => p.PennySum);

    public Sum Sum => new(PennySum);
}