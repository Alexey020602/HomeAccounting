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
}