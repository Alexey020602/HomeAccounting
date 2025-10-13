namespace MyBudgets.Budgets.Data;

abstract class Spending
{
    public SpendingId Id { get; private set; }
    public DateTime PurchaseDate { get; private set; }
    public DateTime AddedDate { get; private set; }
    public abstract int Sum { get; }
    public abstract string Description { get; }

    protected Spending()
    {
        
    }
    public Spending(SpendingId id, DateTime purchaseDate, DateTime addedDate)
    {
        Id = id;
        PurchaseDate = purchaseDate;
        AddedDate = addedDate;
    }
}