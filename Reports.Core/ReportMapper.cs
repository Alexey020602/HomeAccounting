using Checks.Contracts;
using Reports.Contracts;

namespace Reports.Core;

public static class ReportMapper
{
    public static ReportDto CreateReport(this IReadOnlyList<Category> categories, ReportRequest reportRequest)
    {
        return new ReportDto
        {
            DateRange = reportRequest.Range,
            Categories = categories
        };
    }
}