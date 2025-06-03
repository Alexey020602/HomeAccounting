using System.Linq.Expressions;
using Core.Model;
using Core.Model.Report;
using Core.Services;
using Checks.DataBase.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Check = Checks.DataBase.Entities.Check;
using DbCheck = Checks.DataBase.Entities.Check;
namespace Checks.DataBase;

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
    public static Expression<Func<Check, bool>> GetPredicate(this ReportRequest request) => 
        check => (!request.Range.Start.HasValue || check.PurchaseDate >= request.Range.Start.Value.ToUniversalTime())
             && (!request.Range.End.HasValue || check.PurchaseDate <= request.Range.End.Value.ToUniversalTime());
}

public class ReportUseCase(ApplicationContext context) : IReportUseCase, IDisposable
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

    public void Dispose()
    {
        context.Dispose();
        GC.SuppressFinalize(this);
    }
}