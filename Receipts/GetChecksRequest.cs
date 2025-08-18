using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Receipts.Contracts;
using Shared.Utils.Model.Dates;

namespace Checks.Api;

public record struct GetChecksRequest(
    Guid? Login = null,
    int? Take = null,
    int? Skip = null)
{
    public GetChecks ConvertToChecksQuery(Guid budgetId, DateRange range) => new GetChecks(budgetId, range, Login, Take, Skip);
}