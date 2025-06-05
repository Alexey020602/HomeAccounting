using System.Linq.Expressions;
using Checks.Contracts;
using Shared.Model.Checks;

namespace Reports.Core;

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
    public static Expression<Func<CheckDto, bool>> GetPredicate(this ReportRequest request) => 
        check => (!request.Range.Start.HasValue || check.PurchaseDate >= request.Range.Start.Value.ToUniversalTime())
             && (!request.Range.End.HasValue || check.PurchaseDate <= request.Range.End.Value.ToUniversalTime());
}

public class ReportUseCase() : IReportUseCase
{
    public async Task<Report> GetReport(ReportRequest request)
    {
        throw new NotImplementedException();
        // return (await context
        //         .Checks
        //         .AsNoTracking()
        //         .Where(request.GetPredicate())
        //         .Include(c => c.Products)
        //         .ThenInclude(p => p.Subcategory)
        //         .ThenInclude(sub => sub.Category)
        //         .ToListAsync())
        //     .CreateReport(request);
    }
}