namespace Budgets.UI.BudgetState;

public abstract class BudgetsStateProvider
{
    public event BudgetStateChangedHandler? BudgetStateChanged; 
    public abstract Task<BudgetState> GetBudgetStateAsync();

    public Task NotifyBudgetStateChanged(Task<BudgetState> task)
    {
        return BudgetStateChanged?.Invoke(task) ??  Task.CompletedTask;
    }
}

public delegate Task BudgetStateChangedHandler(Task<BudgetState> task);