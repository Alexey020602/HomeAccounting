namespace Budgets.Core.GetBudgets;

public record BudgetsRequest(Guid UserId)
{
    public static implicit operator BudgetsRequest(GetBudgetsQuery query) => new BudgetsRequest(query.UserId);
}