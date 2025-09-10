using Fns.Contracts;
using Fns.Contracts.ReceiptData;
using MaybeResults;
using Rebus.Bus;
using Receipts.Contracts;
using Receipts.Core.ReceiptCategorization;

namespace Receipts.Core.AddReceipt;

public sealed class ReceiptService(ICheckReceiptService checkReceiptService, IReceiptDataService receiptDataService, IBus bus)
    : IReceiptService
{
    public async Task<IMaybe> AddCheckAsync(AddCheckCommand command, CancellationToken token = default)
    {
        if (await checkReceiptService.CheckExistAsync(command.ReceiptData.FiscalData, token)) return Maybe.Create();

        switch ((await receiptDataService.GetReceipt(command.ReceiptData.FiscalData)).Map(receipt => receipt as Receipt))
        {
            case INone<Receipt> error:
                return error;
            case Some<Receipt> success:
                await bus.Send(new ReceiptDataReceived()
                    {
                        ReceiptData = command.ReceiptData,
                        Products = success.Value.Products
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
                return Maybe.Create(success.Value);
            default:
                throw new InvalidOperationException($"Unknown IMaybe<{nameof(Receipt)}> state)");
        }
    }
}