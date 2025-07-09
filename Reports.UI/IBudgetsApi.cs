using Refit;
using Shared.Blazor.Attributes;

namespace Reports.UI;

[ApiAuthorizable("budgets")]
[Headers("Authorization: Bearer")]
public interface IBudgetsApi
{
    [Get("/period")] Task<int> GetPeriodAsync();
}