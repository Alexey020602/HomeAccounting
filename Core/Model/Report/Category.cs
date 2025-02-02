namespace Core.Model.Report;

public sealed class Category
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyList<Subcategory> Subcategories { get; init; }
}