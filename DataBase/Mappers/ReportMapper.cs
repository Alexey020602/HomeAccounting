using Core.Model.Report;
using DataBase.Mappers;
using Check = DataBase.Entities.Check;

namespace Core.Mappers;

public static class ReportMapper
{
    public static Report CreateReport(this IReadOnlyList<Check> checks, ReportRequest reportRequest, int firstDay)
    {
        return new Report
        {
            FirstDay = firstDay,
            Month = reportRequest.Month,
            Year = reportRequest.Year,
            Categories = checks.SelectMany(c => c.Products).ConvertToCategories().ToList()
        };
    }
}