using Core.Mappers;
using Core.Model.Report;
using Core.Services;
using DataBase;
using Microsoft.EntityFrameworkCore;
using Check = Core.Model.ChecksList.Check;

namespace Core;

public class ReportUseCase(ApplicationContext context) : IReportUseCase
{
    public async Task<IReadOnlyList<Check>> GetChecksAsync(int skip = 0, int take = 100)
    {
        var checks = await context.Checks
            .Include(c => c.Products)
            .ThenInclude(p => p.Subcategory)
            .ThenInclude(sub => sub.Category)
            .Skip(skip)
            .Take(take)
            .ToListAsync();


        return checks.ConvertAll(check => check.ConvertToCheckList());
    }

    public async Task<Report> GetReport(ReportRequest request)
    {
        return (await context
                .Checks
                .Where(check => check.PurchaseDate >= request.StartDate.ToUniversalTime() &&
                                check.PurchaseDate <= request.EndDate.ToUniversalTime())
                .Include(c => c.Products)
                .ThenInclude(p => p.Subcategory)
                .ThenInclude(sub => sub.Category)
                .Skip(request.Skip)
                .Take(request.Take)
                .ToListAsync())
            .CreateReport(request);
    }
}