namespace MyBudgets.Budgets.Data;

sealed class ManualSpending : Spending
{
    private string description;
    public override string Description => description;
    private int sum;
    public override int Sum => sum;

    private ManualSpending()
    {
        description = string.Empty;
    }
    public ManualSpending(
        SpendingId id,
        int sum, 
        DateTime purchaseDate, 
        DateTime addedDate, 
        string description) : base(id, purchaseDate, addedDate)
    {
        this.sum = sum;
        this.description = description;
    }
}