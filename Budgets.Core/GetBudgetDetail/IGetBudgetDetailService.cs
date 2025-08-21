using Budgets.Contracts.GetBudgetDetail;
using Budgets.Core.Model;
using MaybeResults;

namespace Budgets.Core.GetBudgetDetail;

public interface IGetBudgetDetailService
{
    Task<IMaybe<Budget>> GetBudgetFullDetail(Guid id, CancellationToken cancellationToken = default);
}