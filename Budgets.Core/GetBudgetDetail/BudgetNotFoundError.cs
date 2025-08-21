using MaybeResults;
using Shared.Utils.Results;

namespace Budgets.Core.GetBudgetDetail;

[None]
public partial record BudgetNotFoundError: INotFoundError
{
    public BudgetNotFoundError(Guid budgetId): this($"Budget with ID {budgetId} not found") { }
}

public partial record BudgetNotFoundError<T>: INotFoundError
{
    public BudgetNotFoundError(Guid budgetId): this($"Budget with ID {budgetId} not found") { }
}