using MaybeResults;
using Receipts.Contracts;

namespace Receipts.Core.AddReceipt;

public interface IReceiptService
{
    public Task<IMaybe> AddCheckAsync(AddCheckCommand command, CancellationToken token = default);
}