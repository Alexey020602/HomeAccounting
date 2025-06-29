namespace Fns.Contracts;

public interface IReceiptDataService
{
    public Task<Receipt> GetReceipt(ReceiptFiscalData  receiptFiscalData);
}

public record Receipt(ReceiptFiscalData ReceiptFiscalData, IReadOnlyList<ReceiptProduct> Products);
public record ReceiptProduct(string Name, double Quantity, int Price, int Sum);