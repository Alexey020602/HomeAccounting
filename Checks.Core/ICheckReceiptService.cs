using Fns.Contracts;

namespace Checks.Core;

public interface ICheckReceiptService
{
    public Task<bool> CheckExistAsync(ReceiptFiscalData data, CancellationToken cancellationToken = default);
}