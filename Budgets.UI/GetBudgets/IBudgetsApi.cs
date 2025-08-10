using Budgets.Contracts.GetBudgets;
using Refit;
using Shared.Blazor.Attributes;

namespace Budgets.UI.GetBudgets;

[ApiAuthorizable("/budgets")]
public interface IBudgetsApi
{
    [Get("")]
    public Task<IReadOnlyCollection<Budget>> GetBudgets(GetBudgetsHttpRequest request);
}