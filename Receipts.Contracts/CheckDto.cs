using Shared.Utils.Model;

namespace Receipts.Contracts;

public class CheckDto
{
    public required int Id { get; init; }
    public required DateTime PurchaseDate { get; init; }
    public required DateTime AddedDate { get; init; }
    public required IReadOnlyList<Category> Categories { get; set; }
    public int PennySum => Categories.Sum(c => c.PennySum);

    public Sum Sum => new(PennySum);
}