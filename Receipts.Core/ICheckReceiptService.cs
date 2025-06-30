using Fns.Contracts;

namespace Receipts.Core;

public interface ICheckReceiptService
{
    public Task<bool> CheckExistAsync(ReceiptFiscalData data, CancellationToken cancellationToken = default);
}