using Core.Model.Report;
using DataBase.Mappers;
using Check = DataBase.Entities.Check;

namespace Core.Mappers;

public static class ReportMapper
{
    public static Report CreateReport(this IReadOnlyList<Check> checks, ReportRequest reportRequest)
    {
        return new Report
        {
            DateRange = reportRequest.Range,
            Categories = checks.SelectMany(c => c.Products).ConvertToCategories().ToList()
        };
    }
}