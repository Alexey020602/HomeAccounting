using Budgets.Core.Model;
using MaybeResults;

namespace Budgets.Core.EditBudget;

public interface IUpdateBudgetService
{
    Task<IMaybe> UpdateBudget(Budget budget, CancellationToken cancellationToken = default);
}