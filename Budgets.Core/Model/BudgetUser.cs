namespace Budgets.Core.Model;

public class BudgetUser
{
    public Guid UserId { get; set; }
    public Guid BudgetId { get; set; }
    public Guid BudgetRoleId { get; set; }
    public Budget? Budget { get; set; }
    public BudgetRole? BudgetRole { get; set; }
}