using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

sealed class ReceiptSpending : Spending
{
    public ReceiptFiscalData FiscalData { get; init; }
    private List<Product> products { get; init; } = [];
    public IReadOnlyList<Product> Products => products;
    public UserId UserId { get; init; }
    public BudgetId BudgetId { get; init; }
    public string PurchasePlace { get; init; }

    public override int Sum => Products.Sum(p => p.Sum);
    public override string Description => PurchasePlace;

    private ReceiptSpending()
    {
        FiscalData = new ReceiptFiscalData(string.Empty, string.Empty, string.Empty);
        PurchasePlace = string.Empty;
    }

    public ReceiptSpending(
        SpendingId id, 
        DateTime purchaseDate, 
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        BudgetId budgetId,
        string purchasePlace,
        IEnumerable<Product> products
    ) : base(id, purchaseDate, addedDate)
    {
        UserId = userId;
        BudgetId = budgetId;
        PurchasePlace = purchasePlace;
        FiscalData = fiscalData;
        if (products.Any())
        {
            this.products.AddRange(products);
        }
    }
}