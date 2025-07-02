using Fns.Contracts;

namespace Receipts.Core.ReceiptCategorization;

public sealed class ReceiptDataReceived
{
    public required string Login { get; set; }
    public required ReceiptFiscalData FiscalData { get; set; }
    public required IReadOnlyList<ReceivedProduct> Products { get; set; }
}