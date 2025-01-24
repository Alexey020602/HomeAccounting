namespace Core.Model;

public class Subcategory
{
    public required int Id { get; init; }
    public required string? Name { get; init; }
    public required Category Category { get; init; }
}