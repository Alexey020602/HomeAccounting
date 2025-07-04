using Receipts.Contracts;
using Reports.Contracts;
using Shared.Model;
using Shared.Model.Dates;

namespace Reports.Core;

public static class ReportMapper
{
    public static ReportDto CreateReport(this IReadOnlyList<Category> categories,DateRange range)
    {
        return new ReportDto
        {
            DateRange = range,
            Categories = categories
        };
    }
}