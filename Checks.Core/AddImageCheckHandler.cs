using Checks.Contracts;
using Checks.Core.BarCode;
using Fns.Contracts;
using Mediator;

namespace Checks.Core;

public sealed class AddImageCheckHandler(IBarcodeService barcodeService, IReceiptService receiptService): ICommandHandler<AddImageCheckCommand>
{
    public async ValueTask<Unit> Handle(AddImageCheckCommand command, CancellationToken cancellationToken)
    {
        var receiptRaw = await barcodeService.ReadBarcodeAsync(command.ImageBytes);
        await receiptService.AddCheckAsync(new AddCheckCommand()
        {
            Login = command.Login,
            FiscalData = new ReceiptFiscalData(receiptRaw)
        }, cancellationToken);
        return Unit.Value;
    }
}