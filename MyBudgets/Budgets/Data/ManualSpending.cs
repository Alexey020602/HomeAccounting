using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

internal sealed partial class ManualSpending : Spending
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
        ) : base(purchaseDate, addedDate, userId)
    {
        this.sum = sum;
        this.description = description;
    }
}