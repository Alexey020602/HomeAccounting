using Fns.Contracts;

namespace Receipts.Core.AddReceipt;

public interface ICheckReceiptService
{
    public Task<bool> CheckExistAsync(ReceiptFiscalData data, CancellationToken cancellationToken = default);
}