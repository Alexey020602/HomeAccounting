using Receipts.Contracts;
using Fns.Contracts;
using Rebus.Bus;

namespace Receipts.Core;

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