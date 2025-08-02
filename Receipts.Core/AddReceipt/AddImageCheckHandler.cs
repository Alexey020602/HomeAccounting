using Fns.Contracts;
using Mediator;
using Receipts.Contracts;
using Receipts.Core.AddReceipt.BarCode;

namespace Receipts.Core.AddReceipt;

public sealed class AddImageCheckHandler(IBarcodeService barcodeService, IReceiptService receiptService): ICommandHandler<AddImageCheckCommand>
{
    public async ValueTask<Unit> Handle(AddImageCheckCommand command, CancellationToken cancellationToken)
    {
        var receiptRaw = await barcodeService.ReadBarcodeAsync(command.ImageBytes);
        await receiptService.AddCheckAsync(new AddCheckCommand()
        {
            ReceiptData = new (new ReceiptFiscalData(receiptRaw), command.UserId),
        }, cancellationToken);
        return Unit.Value;
    }
}