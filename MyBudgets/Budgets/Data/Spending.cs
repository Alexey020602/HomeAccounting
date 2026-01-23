using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

internal abstract class Spending
{
    public SpendingId Id { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public DateTime AddedDate { get; private set; }
    public UserId UserId { get; private set; }
    public abstract Money Sum { get; }
    public abstract string Description { get; }
    protected Spending()
    {
        
    }
    public Spending(DateTime purchaseDate, DateTime addedDate, UserId userId)
    {
        PurchaseDate = purchaseDate;
        AddedDate = addedDate;
        UserId = userId;
    }
}