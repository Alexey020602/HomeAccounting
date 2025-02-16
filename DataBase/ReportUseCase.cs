using Core.Mappers;
using Core.Model.Report;
using Core.Services;
using Microsoft.EntityFrameworkCore;

namespace DataBase;

public class ReportUseCase(ApplicationContext context) : IReportUseCase
{
    public async Task<Report> GetReport(ReportRequest request)
    {
        return (await context
                .Checks
                .Where(check => check.PurchaseDate >= request.StartDate.ToUniversalTime() &&
                                check.PurchaseDate <= request.EndDate.ToUniversalTime())
                .Include(c => c.Products)
                .ThenInclude(p => p.Subcategory)
                .ThenInclude(sub => sub.Category)
                .ToListAsync())
            .CreateReport(request);
    }
}