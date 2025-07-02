using Receipts.Core.GetReceipts;

namespace Receipts.Core.ReceiptCategorization;

public interface IReceiptSaveService
{
    public Task SaveReceipt(AddCheckRequest addCheckRequest);
}