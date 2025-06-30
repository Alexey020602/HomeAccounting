using Receipts.Contracts;
using Reports.Contracts;

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


static class ReportRequestExtensions
{
    public static GetChecksQuery ToGetCheckQuery(this ReportRequest reportRequest) => new(Range: reportRequest.Range);
}
public class ReportUseCase(ICheckUseCase checkUseCase) : IReportUseCase
{
    private readonly ICheckUseCase checkUseCase = checkUseCase;
    public async Task<ReportDto> GetReport(ReportRequest request)
    {
        var checks = await checkUseCase.GetCategoriesAsync(request.ToGetCheckQuery());
        return checks.CreateReport(request);
    }
}