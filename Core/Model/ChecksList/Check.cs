namespace Core.Model.ChecksList;

public class Check
{
    public required int Id { get; init; }
    public required DateTime PurchaseDate { get; init; }
    public required DateTime AddedDate { get; init; }
    public required IReadOnlyList<Category> Categories { get; set; }
    public int PennySum => Categories.Sum(c => c.PennySum);

    public Sum Sum => new(PennySum);
}