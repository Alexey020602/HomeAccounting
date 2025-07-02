using Receipts.Core;
using Fns.Contracts;
using Receipts.Core.AddReceipt;

namespace Receipts.DataBase;

public sealed class CheckReceiptService(ReceiptsContext receiptsContext): ICheckReceiptService
{
    public Task<bool> CheckExistAsync(ReceiptFiscalData data, CancellationToken cancellationToken = default) =>
        receiptsContext.CheckIsCheckExists(data, cancellationToken);
}