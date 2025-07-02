using Mediator;
using Microsoft.Extensions.DependencyInjection;
using Reports.Contracts;
using Reports.Core;

namespace Reports.Api;

public static class ReportsModule
{
    public static IServiceCollection AddReportsModule(this IServiceCollection services) => 
        services.AddScoped<IQueryHandler<GetPeriodReportQuery, ReportDto>, GetReportHandler>();
}