using Fns.Contracts;
using Fns.Contracts.ReceiptData;
using Rebus.Bus;
using Receipts.Contracts;
using Receipts.Core.ReceiptCategorization;

namespace Receipts.Core.AddReceipt;

public sealed class ReceiptService(ICheckReceiptService checkReceiptService, IReceiptDataService receiptDataService, IBus bus)
    : IReceiptService
{
    public async Task AddCheckAsync(AddCheckCommand command, CancellationToken token = default)
    {
        if (await checkReceiptService.CheckExistAsync(command.FiscalData, token)) return;

        var receipt = await receiptDataService.GetReceipt(command.FiscalData);
        await bus.Send(new ReceiptDataReceived()
            {
                Login = command.Login,
                FiscalData = receipt.ReceiptFiscalData,
                Products = receipt.Products
                    .Select(p => new ReceivedProduct()
                    {
                        Name = p.Name,
                        Price = p.Price,
                        Quantity = p.Quantity,
                        Sum = p.Sum
                    })
                    .ToList()
            }
        );
    }
}