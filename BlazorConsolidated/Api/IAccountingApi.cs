using Refit;
using Shared.Blazor.Attributes;

namespace BlazorConsolidated.Api;

[ApiAuthorizable("accounting")]
[Headers("Authorization: Bearer")]
public interface IAccountingApi
{
    [Get("/period")] Task<int> GetPeriodAsync();
}