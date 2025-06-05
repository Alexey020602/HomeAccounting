using Core.Model.Report;
using Core.Reports.Model;
using Check = Checks.DataBase.Entities.Check;
using Entities_Check = Checks.DataBase.Entities.Check;

namespace Checks.DataBase.Mappers;

public static class ReportMapper
{
    public static Report CreateReport(this IReadOnlyList<Entities_Check> checks, ReportRequest reportRequest)
    {
        return new Report
        {
            DateRange = reportRequest.Range,
            Categories = checks.SelectMany(c => c.Products).ConvertToCategories().ToList()
        };
    }
}