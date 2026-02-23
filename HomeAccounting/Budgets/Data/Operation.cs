using ClientServerShared.Model;
using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using HomeAccounting.Common.Model.ValueObjects;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

internal sealed partial class Operation : Entity<OperationId>
{
    private string description = string.Empty;
    private Money sum;

    public DateTimeOffset PurchaseDate { get; private set; }
    public DateTimeOffset AddedDate { get; private set; }
    public UserId UserId { get; private set; }
    public Money Sum => sum;
    public string Description => description;
    public CategoryId? CategoryId { get; private set; }

    private Operation() { }

    public Operation(
        Money sum,
        DateTimeOffset purchaseDate,
        CategoryId? categoryId,
        DateTimeOffset addedDate,
        string description,
        UserId userId)
    {
        this.sum = sum;
        PurchaseDate = purchaseDate;
        AddedDate = addedDate;
        UserId = userId;
        this.description = description;
        CategoryId = categoryId;
    }
}
