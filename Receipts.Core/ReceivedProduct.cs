namespace Receipts.Core;

public sealed class ReceivedProduct
{
    public required string Name { get; set; }
    public required int Price { get; set; }
    public required double Quantity { get; set; }
    public required int Sum { get; set; }
}