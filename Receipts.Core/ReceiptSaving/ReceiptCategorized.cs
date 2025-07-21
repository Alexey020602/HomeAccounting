using Fns.Contracts;
using Receipts.Contracts;

namespace Receipts.Core.ReceiptSaving;

public sealed class ReceiptCategorized
{
    public required ReceiptData ReceiptData { get; init; }
    public required IReadOnlyList<CategorizedProduct> Products { get; set; }
}