using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

sealed class BudgetUser
{
    public UserId UserId { get; private set; }
    public Budget Budget { get; private set; }
    public BudgetRole BudgetRole { get; private set; }

    private BudgetUser()
    {
        Budget = new Budget();
        BudgetRole = new BudgetRole();
    }

    public BudgetUser(UserId userId, Budget budget, BudgetRole budgetRole)
    {
        UserId = userId;
        Budget = budget;
        BudgetRole = budgetRole;
    }
}