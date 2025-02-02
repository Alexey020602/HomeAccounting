namespace Core.Model.Report;

public sealed class Subcategory
{
    public required int Id { get; init; }
    public required string? Name { get; init; }
    public required IReadOnlyList<Product> Products { get; init; }
}