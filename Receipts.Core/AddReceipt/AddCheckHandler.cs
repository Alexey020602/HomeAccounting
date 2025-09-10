using MaybeResults;
using Mediator;
using Receipts.Contracts;
using Shared.Utils.MediatorWithResults;

namespace Receipts.Core.AddReceipt;

public sealed class AddCheckHandler(IReceiptService receiptService) : IResultCommandHandler<AddCheckCommand>
{
    public ValueTask<IMaybe> Handle(AddCheckCommand command, CancellationToken cancellationToken) =>
        new (receiptService.AddCheckAsync(command, cancellationToken));
}