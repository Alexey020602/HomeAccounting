using Checks.Contracts;

namespace Checks.Core;

public interface IReceiptService
{
    public Task AddCheckAsync(AddCheckCommand command, CancellationToken token = default);
}