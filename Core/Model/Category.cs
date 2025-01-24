namespace Core.Model;

public class Category
{
    public required int Id { get; init; }
    public required string Name { get; init; }
    public override string ToString() => Name;
}