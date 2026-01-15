using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

internal sealed class Budget
{
    private List<BudgetUser> budgetUsers = [];
    private List<Spending> spendings = [];
    public BudgetId Id { get; set; }
    public string Name { get; set; }
    public int BeginOfPeriod { get; set; }
    public int? Limit { get; set; }
    public UserId CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public IReadOnlyList<BudgetUser> BudgetUsers => budgetUsers;
    public IReadOnlyList<Spending> Spendings => spendings;
    internal Budget()
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
    
    
}