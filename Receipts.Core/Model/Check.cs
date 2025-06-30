namespace Receipts.Core.Model;

public record Check
{
    public required int Id { get; set; }
    public required DateTime PurchaseDate { get; set; }
    public required DateTime AddedDate { get; set; }
    public required string Fd { get; set; }
    public required string Fn { get; set; }
    public required string Fp { get; set; }
    public required string S { get; set; }
    public required IReadOnlyList<Product> Products { get; set; } = [];
}