namespace Budgets.DataBase.Entities;

public class Invitation
{
    public Guid Id { get; set; }
    public Guid BudgetId { get; set; }
    public Guid UserId { get; set; }
}

