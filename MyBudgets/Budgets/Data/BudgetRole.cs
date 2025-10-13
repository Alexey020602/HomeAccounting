namespace MyBudgets.Budgets.Data;

class BudgetRole
{
    public const string OwnerRoleName = "Владелец";
    public const string AdminRoleName = "Администратор";
    public const string UserRoleName = "Пользователь";
    public BudgetRoleId Id { get; private set; }
    public string Name { get; private set; }
    public BudgetPermissions Permissions { get; private set; }
    public IReadOnlyList<BudgetUser> BudgetUsers => budgetUsers;
    private List<BudgetUser> budgetUsers = [];
    internal BudgetRole()
    {
        Name = string.Empty;
    }

    public BudgetRole(string name,  BudgetPermissions permissions, IEnumerable<BudgetUser> budgetUsers)
    {
        Name = name;
        Permissions = permissions;
        if (budgetUsers.Any())
        {
            this.budgetUsers.AddRange(budgetUsers);
        }
    }
}