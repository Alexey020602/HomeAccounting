namespace Core.Model.Report;

public sealed class Check
{
    public required int Id { get; init; }
    public DateTime AddedDate { get; init; }
    public DateTime PurchaseDate { get; init; }
}