using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;
sealed class Budget
{
    public BudgetId Id { get; set; }
    public string Name { get; set; }
    public int BeginOfPeriod { get; set; }
    public int? Limit { get; set; }
    public UserId CreatorId { get; set; }
    public DateTime CreationDate { get; set; }
    public List<BudgetUser> BudgetUsers { get; set; } = [];

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
            BudgetUsers.AddRange(budgetUsers);
        }
    }
}