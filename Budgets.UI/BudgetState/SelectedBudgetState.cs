namespace Budgets.UI.BudgetState;

public class SelectedBudgetState(Guid budgetId, string name): BudgetState
{
    public Guid BudgetId { get; } = budgetId;
    public override string Name => name;
}