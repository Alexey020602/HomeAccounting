using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using MyBudgets.Common.Model;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

sealed partial class ReceiptSpending : Spending
{
    public ReceiptFiscalData FiscalData { get; private set; }
    private List<Product> products = [];
    public IReadOnlyList<Product> Products => products;
    public string PurchasePlace { get; private set; }

    public override Money Sum => Products.Sum(p => p.Sum);
    public override string Description => PurchasePlace;

    
    private ReceiptSpending()
    {
        FiscalData = ReceiptFiscalData.Empty();
        PurchasePlace = string.Empty;
    }

    public ReceiptSpending(
        DateTime purchaseDate, 
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs
    ) : base(purchaseDate, addedDate, userId)
    {
        PurchasePlace = purchasePlace;
        FiscalData = fiscalData;

        var productsToAdd = productInputs
            .Select(product => new Product(product.Name, product.Quantity, product.Price, product.Sum, product.CategoryId));

        if (productsToAdd.Any())
        {
            products.AddRange(productsToAdd);
        }
        
        // sum = products.Sum(p => p.Sum);
    }
}