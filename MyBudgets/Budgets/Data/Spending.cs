using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

internal abstract class Spending
{
    public SpendingId Id { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public DateTime AddedDate { get; private set; }
    public UserId UserId { get; private set; }
    // public BudgetId BudgetId { get; private set ; }
    public abstract int Sum { get; }
    public abstract string Description { get; }

    protected Spending()
    {
        
    }
    public Spending(/*SpendingId id, */DateTime purchaseDate, DateTime addedDate, UserId userId/*, BudgetId budgetId*/)
    {
        PurchaseDate = purchaseDate;
        AddedDate = addedDate;
        UserId = userId;
        // BudgetId = budgetId;
    }
}