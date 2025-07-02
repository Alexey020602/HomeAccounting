using Mediator;
using Receipts.Contracts;
using Reports.Contracts;
using Shared.Model;

namespace Reports.Core;

public record GetPeriodReportQuery(DateRange Range, string? Login = null) : IQuery<ReportDto>
{
    public static explicit operator GetChecks (GetPeriodReportQuery query) => new(query.Range,  query.Login);
};