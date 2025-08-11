namespace Budgets.UI.BudgetState;
public abstract class BudgetsStateProvider
{
    public event Action<Task<BudgetState>>? BudgetStateChanged; 
    public abstract Task<BudgetState> GetBudgetStateAsync();

    public void NotifyBudgetStateChanged(Task<BudgetState> task)
    {
        BudgetStateChanged?.Invoke(task);
    }
}

public class BudgetState
{
    
}

public class SelectedBudgetState(Guid budgetId): BudgetState
{
    public Guid BudgetId { get; } = budgetId;
}

public delegate void BudgetStateChangedHandler(Task<BudgetState> task);