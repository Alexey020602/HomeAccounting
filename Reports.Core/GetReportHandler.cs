using Mediator;
using Receipts.Contracts;
using Reports.Contracts;

namespace Reports.Core;

public sealed class GetReportHandler(IGetProductsService getProductsService): IQueryHandler<GetPeriodReportQuery, ReportDto>
{
    public async ValueTask<ReportDto> Handle(GetPeriodReportQuery query, CancellationToken cancellationToken)
    {
        var categories =  await getProductsService.GetProductsAsync((GetChecks)query);
        return categories.CreateReport(query.Range);
    }
}