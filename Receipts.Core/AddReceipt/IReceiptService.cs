using Receipts.Contracts;

namespace Receipts.Core.AddReceipt;

public interface IReceiptService
{
    public Task AddCheckAsync(AddCheckCommand command, CancellationToken token = default);
}