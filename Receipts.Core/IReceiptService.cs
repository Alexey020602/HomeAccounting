using Receipts.Contracts;

namespace Receipts.Core;

public interface IReceiptService
{
    public Task AddCheckAsync(AddCheckCommand command, CancellationToken token = default);
}