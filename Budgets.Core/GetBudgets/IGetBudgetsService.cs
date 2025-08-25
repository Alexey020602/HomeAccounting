using Budgets.Contracts.GetBudgets;
using MaybeResults;

namespace Budgets.Core.GetBudgets;

public interface IGetBudgetsService
{
    public Task<IMaybe<IReadOnlyCollection<Budget>>> GetBudgets(BudgetsRequest request);
}