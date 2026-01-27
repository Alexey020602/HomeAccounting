using ClientServerShared.Model;
using ClientServerShared.Model.Money;
using HomeAccounting.Categories.Data;
using HomeAccounting.Common.Model;
using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

internal sealed partial class Budget: Entity<BudgetId>
{
    private List<BudgetUser> budgetUsers = [];
    private List<Spending> spendings = [];
    public string Name { get; private set; }
    public int BeginOfPeriod { get; private set; }
    public Money? Limit { get; private set; }
    public UserId CreatorId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public IReadOnlyList<BudgetUser> BudgetUsers => budgetUsers;
    public IReadOnlyList<Spending> Spendings => spendings;
    private Budget()
    {
        Name = string.Empty;
    }

    public Budget(string name, int beginOfPeriod, Money? limit, UserId userId, DateTime creationDate, IEnumerable<BudgetUser> budgetUsers)
    {
        Name = name;
        BeginOfPeriod = beginOfPeriod;
        Limit = limit;
        CreatorId = userId;
        CreationDate = creationDate;

        if (budgetUsers.Any())
        {
            this.budgetUsers.AddRange(budgetUsers);
        }
    }

    public void AddManualSpending(Money sum, string description, CategoryId? categoryId, DateTime purchaseDate, DateTime addedDate, UserId userId)
    {
        spendings.Add(new ManualSpending(sum,  purchaseDate, categoryId, addedDate, description, userId));
    }

    public void AddReceiptSpending(
        DateTime purchaseDate,
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        string purchasePlace,
        IEnumerable<ProductInput> productInputs)
    {
        spendings.Add(new ReceiptSpending(
            purchaseDate,
            addedDate,
            fiscalData,
            userId,
            purchasePlace,
            productInputs));
    }

    public void AddReceiptSpending(
        DateTime purchaseDate,
        DateTime addedDate,
        ReceiptFiscalData fiscalData,
        UserId userId,
        Money declaredSum)
    {
        spendings.Add(new ReceiptSpending(
            purchaseDate,
            addedDate,
            fiscalData,
            userId,
            declaredSum));
    }

    public void ChangeCategoryForProduct(SpendingId spendingId, ProductId productId, CategoryId categoryId)
    {
        var spending = GetReceiptSpending(spendingId);
        
        spending.ChangeCategoryForProduct(productId, categoryId);
    }

    public void DeleteCategoryForProduct(SpendingId spendingId, ProductId productId)
    {
        var spending = GetReceiptSpending(spendingId);
        spending.DeleteCategoryForProduct(productId);
    }

    public void Update(string name, int beginOfPeriod, Money? limit)
    {
        Name = name;
        BeginOfPeriod = beginOfPeriod;
        Limit = limit;
    }

    public BudgetRoleId? GetUserRole(UserId userId)
    {
        return budgetUsers.FirstOrDefault(bu => bu.UserId == userId)?.BudgetRoleId;
    }

    public void AddUser(UserId userId, BudgetRoleId roleId)
    {
        if (budgetUsers.Any(bu => bu.UserId == userId))
        {
            throw new InvalidOperationException("User is already added to this budget");
        }

        budgetUsers.Add(new BudgetUser(userId, roleId));
    }

    /// <summary>
    /// Удаление пользователя из бюджета
    /// Нельзя удалить владельца
    /// Нельзя удалить самого себя
    /// Нельзя админу удалить другого админа
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="currentUserId"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void RemoveUser(UserId userId, UserId currentUserId)
    {
        if (userId == currentUserId)
        {
            throw new DomainException("Cannot remove yourself");
        }

        if (budgetUsers.FirstOrDefault(bu => bu.UserId == userId) is not { } user)
        {
            throw new DomainException("User is not in this budget");
        }

        if (user.IsOwner)
        {
            throw new DomainException("Cannot remove owner");
        }

        if (budgetUsers.FirstOrDefault(bu => bu.UserId == currentUserId) is not { } currentUser)
        {
            throw new DomainException("Current user is not in this budget");
        }

        if (currentUser.IsAdmin &&user.IsAdmin)
        {
            throw new DomainException("Admin cannot be deleted by admin");
        }

        budgetUsers.Remove(user);
    }
    
    private Spending GetSpending(SpendingId spendingId) => spendings.FirstOrDefault(sp => sp.Id == spendingId) ?? throw new DomainException("Spending not found");

    private ManualSpending GetManualSpending(SpendingId spendingId)
    {
        if (GetSpending(spendingId) is not ManualSpending manualSpending)
        {
            throw new DomainException("This spending is not manual");
        }
        
        return manualSpending;
    }

    private ReceiptSpending GetReceiptSpending(SpendingId spendingId)
    {
        if (GetSpending(spendingId) is not ReceiptSpending receiptSpending)
        {
            throw new DomainException("This spending is not receipt");
        }

        return receiptSpending;
    }
}