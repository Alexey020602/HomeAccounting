using System.Linq.Expressions;
using Core.Mappers;
using Core.Model;
using Core.Model.ChecksList;
using Core.Model.Report;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using DbCheck = DataBase.Entities.Check;
namespace DataBase;

public interface IAccountingSettingsService
{
    Task<int> GetFirstDayOfAccountingPeriod();
}

public static class AccountingSettingsServiceExtensions
{
    public static async Task<DateRange> GetPeriodByMonth(this IAccountingSettingsService accountingSettingsService,
        int month, int year)
    {
        var firstDay = await accountingSettingsService.GetFirstDayOfAccountingPeriod();

        var startDay = new DateTime(year, month, firstDay);
        var endDay = startDay.AddMonths(1);

        return new DateRange()
        {
            StartDate = startDay,
            EndDate = endDay
        };
    }
}

public sealed class DateRange
{
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }

    public Expression<Func<DbCheck, bool>> CheckFilter =>
        check => check.PurchaseDate >= StartDate.ToUniversalTime()
                 && check.PurchaseDate <= EndDate.ToUniversalTime();
}

public sealed class ConstantAccountingSettingsService : IAccountingSettingsService
{
    private const int FirstDayOfAccountingPeriod = 7;
    public Task<int> GetFirstDayOfAccountingPeriod() => Task.FromResult(FirstDayOfAccountingPeriod);
}

public class ReportUseCase(ApplicationContext context) : IReportUseCase
{
    private readonly IAccountingSettingsService _accountingSettingsService = new ConstantAccountingSettingsService();

    public async Task<Report> GetReport(ReportRequest request)
    {
        var dateRange = await _accountingSettingsService.GetPeriodByMonth(request.Month, request.Year);
        return (await context
                .Checks
                .AsNoTracking()
                .Where(dateRange.CheckFilter)
                .Include(c => c.Products)
                .ThenInclude(p => p.Subcategory)
                .ThenInclude(sub => sub.Category)
                .ToListAsync())
            .CreateReport(request, dateRange.StartDate.Day);
    }
}