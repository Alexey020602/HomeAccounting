using Core.Mappers;
using Core.Model;
using DataBase;
using Microsoft.EntityFrameworkCore;

namespace Core;

public class ReportUseCase(ApplicationContext context): IReportUseCase
{
    public async Task<IReadOnlyList<Check>> GetChecksAsync()
    {
        var checks = await context.Checks
            .Include(c => c.Products)
            .ThenInclude(p => p.Subcategory)
            .ThenInclude(sub => sub.Category)
            .ToListAsync();

        return checks.ConvertAll(check => check.ConvertToCheck());
    }
}