namespace Checks.Contracts;

public class Category
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required IReadOnlyList<Subcategory> Subcategories { get; set; }
    public int PennySum => Subcategories.Sum(s => s.PennySum);

    public Sum Sum => new(PennySum);
}