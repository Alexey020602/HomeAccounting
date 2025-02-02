using Core.Model.Report;
using Check = DataBase.Entities.Check;

namespace Core.Mappers;

public static class ReportMapper
{
    public static Report CreateReport(this IReadOnlyList<Check> checks, ReportRequest reportRequest)
    {
        return new Report
        {
            StartDate = reportRequest.StartDate,
            EndDate = reportRequest.EndDate,
            Categories = checks.SelectMany(c => c.Products).ConvertToCategories().ToList()
        };
    }

    // public static
}