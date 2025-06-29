using Checks.Core;
using Fns.Contracts;

namespace Checks.DataBase;

public sealed class CheckReceiptService(ChecksContext checksContext): ICheckReceiptService
{
    public Task<bool> CheckExistAsync(ReceiptFiscalData data, CancellationToken cancellationToken = default) =>
        checksContext.CheckIsCheckExists(data, cancellationToken);
}