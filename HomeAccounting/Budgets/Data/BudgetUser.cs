using HomeAccounting.Users.Data;

namespace HomeAccounting.Budgets.Data;

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

    public bool IsOwner => BudgetRoleId == BudgetRole.OwnerBudgetRoleId;
    public bool IsAdmin => BudgetRoleId == BudgetRole.AdminBudgetRoleId;
}