using MyBudgets.Common.Model;
using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

internal sealed partial class Budget
{
    private List<BudgetUser> budgetUsers = [];
    private List<Spending> spendings = [];
    public BudgetId Id { get; private set; }
    public string Name { get; private set; }
    public int BeginOfPeriod { get; private set; }
    public int? Limit { get; private set; }
    public UserId CreatorId { get; private set; }
    public DateTime CreationDate { get; private set; }
    public IReadOnlyList<BudgetUser> BudgetUsers => budgetUsers;
    public IReadOnlyList<Spending> Spendings => spendings;
    private Budget()
    {
        Name = string.Empty;
    }

    public Budget(string name, int beginOfPeriod, int? limit, UserId userId, DateTime creationDate, IEnumerable<BudgetUser> budgetUsers)
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

    public void AddManualSpending(int sum, string description, DateTime purchaseDate, DateTime addedDate, UserId userId)
    {
        spendings.Add(new ManualSpending(sum, purchaseDate, addedDate, description, userId));
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

    public void Update(string name, int beginOfPeriod, int? limit)
    {
        Name = name;
        BeginOfPeriod = beginOfPeriod;
        Limit = limit;
    }

    // public bool CanUserEdit(UserId userId, BudgetRole? userRole)
    // {
    //     if (userRole is null)
    //         return false;
    //     
    //     return userRole.Permissions.Contains(BudgetPermissions.Edit);
    // }
    //
    // public bool CanUserDelete(UserId userId, BudgetRole? userRole)
    // {
    //     if (userRole is null)
    //         return false;
    //     
    //     return userRole.Permissions.Contains(BudgetPermissions.Delete);
    // }

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
}