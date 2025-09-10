using Fns.Contracts;
using MaybeResults;
using Mediator;
using Receipts.Contracts;
using Receipts.Core.AddReceipt.BarCode;
using Shared.Utils.MediatorWithResults;

namespace Receipts.Core.AddReceipt;

public sealed class AddImageCheckHandler(IBarcodeService barcodeService, IReceiptService receiptService): IResultCommandHandler<AddImageCheckCommand>
{
    public async ValueTask<IMaybe> Handle(AddImageCheckCommand command, CancellationToken cancellationToken)
    {
        var receiptRaw = await barcodeService.ReadBarcodeAsync(command.ImageBytes); 
        return await receiptService.AddCheckAsync(new AddCheckCommand()
        {
            ReceiptData = new (new ReceiptFiscalData(receiptRaw), command.UserId, command.BudgetId),
        }, cancellationToken);
    }
}