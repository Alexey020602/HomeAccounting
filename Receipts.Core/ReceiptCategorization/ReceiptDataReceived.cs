using Fns.Contracts;
using Receipts.Contracts;

namespace Receipts.Core.ReceiptCategorization;

public sealed class ReceiptDataReceived
{
    public required ReceiptData  ReceiptData { get; init; }
    public required IReadOnlyList<ReceivedProduct> Products { get; set; }
}