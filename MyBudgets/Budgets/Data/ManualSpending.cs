using MyBudgets.Users.Data;

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
        int sum, 
        DateTime purchaseDate, 
        DateTime addedDate, 
        string description,
        UserId userId
        // BudgetId budgetId
        ) : base(purchaseDate, addedDate, userId/*, budgetId*/)
    {
        this.sum = sum;
        this.description = description;
    }
}