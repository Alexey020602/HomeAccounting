using MyBudgets.Users.Data;

namespace MyBudgets.Budgets.Data;

sealed class BudgetUser
{
    public UserId UserId { get; private set; }
    // public Budget Budget { get; private set; }
    public BudgetRoleId BudgetRoleId { get; private set; }

    private BudgetUser()
    {
    }

    public BudgetUser(UserId userId, BudgetRoleId budgetRoleId)
    {
        UserId = userId;
        // Budget = budget;
        BudgetRoleId = budgetRoleId;
    }
}