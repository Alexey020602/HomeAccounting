using Refit;
using Shared.Blazor.Attributes;

namespace Reports.UI;

[ApiAuthorizable("accounting")]
[Headers("Authorization: Bearer")]
public interface IAccountingApi
{
    [Get("/period")] Task<int> GetPeriodAsync();
}