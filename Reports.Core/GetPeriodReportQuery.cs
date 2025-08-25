using Mediator;
using Receipts.Contracts;
using Reports.Contracts;
using Shared.Utils.Model.Dates;

namespace Reports.Core;

public record GetPeriodReportQuery(Guid BudgetId, DateRange Range, Guid? UserId = null) : IQuery<ReportDto>
{
    public static explicit operator GetChecks (GetPeriodReportQuery query) => new(query.BudgetId, query.Range, query.UserId);
};