using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

internal abstract class Spending: Entity<SpendingId>
{
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