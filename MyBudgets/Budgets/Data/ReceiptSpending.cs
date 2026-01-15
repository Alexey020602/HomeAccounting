using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

sealed class ReceiptSpending : Spending
{
    public ReceiptFiscalData FiscalData { get; private set; }
    private List<Product> products = [];
    public IReadOnlyList<Product> Products => products;
    public string PurchasePlace { get; private set; }

    public override int Sum => Products.Sum(p => p.Sum);
    public override string Description => PurchasePlace;

    private ReceiptSpending()
    {
        FiscalData = new ReceiptFiscalData(string.Empty, string.Empty, string.Empty);
        PurchasePlace = string.Empty;
    }

    public ReceiptSpending(
        DateTime purchaseDate, 
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        // BudgetId budgetId,
        string purchasePlace,
        IEnumerable<Product> products
    ) : base(purchaseDate, addedDate, userId/*, budgetId*/)
    {
        PurchasePlace = purchasePlace;
        FiscalData = fiscalData;
        if (products.Any())
        {
            this.products.AddRange(products);
        }
    }
}