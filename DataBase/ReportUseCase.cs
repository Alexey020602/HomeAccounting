using System.Linq.Expressions;
using Core.Mappers;
using Core.Model;
using Core.Model.ChecksList;
using Core.Model.Report;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbCheck = DataBase.Entities.Check;
namespace DataBase;

// public static class AccountingSettingsServiceExtensions
// {
//     public static async Task<DateRange> GetPeriodByMonth(this IAccountingSettingsService accountingSettingsService,
//         int month, int year)
//     {
//         var firstDay = await accountingSettingsService.GetFirstDayOfAccountingPeriod();
//
//         var startDay = new DateTime(year, month, firstDay);
//         var endDay = startDay.AddMonths(1);
//
//         return new DateRange()
//         {
//             StartDate = startDay,
//             EndDate = endDay
//         };
//     }
// }

public static class ReportRequestExtensions
{
    public static Expression<Func<DbCheck, bool>> GetPredicate(this ReportRequest request) => 
        check => check.PurchaseDate >= request.Range.Start.ToUniversalTime()
        && check.PurchaseDate <= request.Range.End.ToUniversalTime();
}

public class ReportUseCase(ApplicationContext context) : IReportUseCase
{
    public async Task<Report> GetReport(ReportRequest request)
    {
        return (await context
                .Checks
                .AsNoTracking()
                .Where(request.GetPredicate())
                .Include(c => c.Products)
                .ThenInclude(p => p.Subcategory)
                .ThenInclude(sub => sub.Category)
                .ToListAsync())
            .CreateReport(request);
    }
}