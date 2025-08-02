using Rebus.Handlers;
using Receipts.Core.GetReceipts;
using Receipts.Core.ReceiptCategorization;

namespace Receipts.Core.ReceiptSaving;

public sealed class ReceiptCategorizedHandler(IReceiptSaveService receiptSaveService): IHandleMessages<ReceiptCategorized>
{
    public Task Handle(ReceiptCategorized message)
    {
        return receiptSaveService.SaveReceipt(new AddCheckRequest(
            message.ReceiptData,
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