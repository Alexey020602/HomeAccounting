using Mediator;
using Receipts.Contracts;

namespace Receipts.Core.AddReceipt;

public sealed class AddCheckHandler(IReceiptService receiptService) : ICommandHandler<AddCheckCommand>
{
    public async ValueTask<Unit> Handle(AddCheckCommand command, CancellationToken cancellationToken)
    {
        await receiptService.AddCheckAsync(command, cancellationToken);
        return Unit.Value;
    }
}