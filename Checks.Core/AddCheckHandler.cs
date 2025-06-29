using Checks.Contracts;
using Mediator;

namespace Checks.Core;

public sealed class AddCheckHandler(IReceiptService receiptService) : ICommandHandler<AddCheckCommand>
{
    public async ValueTask<Unit> Handle(AddCheckCommand command, CancellationToken cancellationToken)
    {
        await receiptService.AddCheckAsync(command, cancellationToken);
        return Unit.Value;
    }
}