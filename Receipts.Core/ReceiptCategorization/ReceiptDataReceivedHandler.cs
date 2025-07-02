using Fns.Contracts;
using Rebus.Bus;
using Rebus.Handlers;
using Receipts.Core.ReceiptSaving;
using CategorizedProduct = Receipts.Core.ReceiptSaving.CategorizedProduct;

namespace Receipts.Core.ReceiptCategorization;

public sealed class ReceiptDataReceivedHandler(IProductCategorizationService categorizationService, IBus bus)
    : IHandleMessages<ReceiptDataReceived>
{
    public async Task Handle(ReceiptDataReceived message)
    {
        var categorized = await categorizationService.CategorizeProducts(new CategorizationRequest(message
            .Products
            .Select(p => new CategorizationProduct(p.Name))
            .ToList()
        ));

        var products =
            from categorizedProduct in categorized.Products
            join fnsProduct in message.Products on categorizedProduct.Name equals fnsProduct.Name
            select new CategorizedProduct()
            {
                Name = fnsProduct.Name,
                Category = categorizedProduct.Category,
                Subcategory = categorizedProduct.Subcategory,
                Price = fnsProduct.Price,
                Quantity = fnsProduct.Quantity,
                Sum = fnsProduct.Sum
            };
        
        await bus.Send(new ReceiptCategorized()
        {
            Login = message.Login,
            FiscalData = message.FiscalData,
            Products = products.ToList()
        });
    }
}