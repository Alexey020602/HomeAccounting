namespace Receipts.Core.ReceiptSaving;

public sealed class CategorizedProduct
{
    public required string Name { get; set; }
    public required int Price { get; set; }
    public required double Quantity { get; set; }
    public required int Sum { get; set; }
    public required string? Subcategory { get; set; }
    public required string Category { get; set; }
}