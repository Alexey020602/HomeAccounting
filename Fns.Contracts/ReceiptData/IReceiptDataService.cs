using MaybeResults;

namespace Fns.Contracts.ReceiptData;

public interface IReceiptDataService
{
    public Task<IMaybe<Receipt>> GetReceipt(ReceiptFiscalData  receiptFiscalData);
}

public record Receipt(ReceiptFiscalData ReceiptFiscalData, IReadOnlyList<ReceiptProduct> Products);
public record ReceiptProduct(string Name, double Quantity, int Price, int Sum);