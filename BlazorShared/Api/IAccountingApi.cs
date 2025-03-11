using BlazorShared.Api.Attributes;
using Refit;

namespace BlazorShared.Api;

[ApiAuthorizable("accounting")]
[Headers("Authorization: Bearer")]
public interface IAccountingApi
{
    [Get("/period")] Task<int> GetPeriodAsync();
}