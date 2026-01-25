using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using MyBudgets.Categories.Data;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

internal sealed partial class ManualSpending : Spending
{
    private string description;
    public override string Description => description;
    private Money sum;
    public override Money Sum => sum;
    public CategoryId? CategoryId  { get; private set; }

    private ManualSpending()
    {
        description = string.Empty;
    }
    public ManualSpending(
        Money sum, 
        DateTime purchaseDate, 
        CategoryId? categoryId,
        DateTime addedDate, 
        string description,
        UserId userId
        ) : base(purchaseDate, addedDate, userId)
    {
        this.sum = sum;
        this.description = description;
        this.CategoryId = categoryId;
    }
}