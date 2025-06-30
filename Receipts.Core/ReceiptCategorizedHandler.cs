using Rebus.Handlers;

namespace Receipts.Core;

public sealed class ReceiptCategorizedHandler(IReceiptSaveService receiptSaveService): IHandleMessages<ReceiptCategorized>
{
    public Task Handle(ReceiptCategorized message)
    {
        return receiptSaveService.SaveReceipt(new AddCheckRequest(
            message.Login,
            message.FiscalData.T,
            message.FiscalData.Fn,
            message.FiscalData.Fd,
            message.FiscalData.Fp,
            message.FiscalData.S,
            message.Products.Select(product => new AddCheckRequest.Product(
                product.Name,
                product.Quantity,
                product.Price,
                product.Sum,
                product.Subcategory,
                product.Category
            )).ToList()
        ));
    }
}