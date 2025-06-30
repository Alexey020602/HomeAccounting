using Fns.Contracts;

namespace Receipts.Core;

public sealed class ReceiptCategorized
{
    public required string Login { get; set; }
    public required ReceiptFiscalData FiscalData { get; set; }
    public required IReadOnlyList<CategorizedProduct> Products { get; set; }
}