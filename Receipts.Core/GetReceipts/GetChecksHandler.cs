using Mediator;
using Receipts.Contracts;
using Receipts.Core.Mappers;

namespace Receipts.Core.GetReceipts;

public sealed class GetChecksHandler(IGetReceiptsService getReceiptsService): IQueryHandler<GetChecks, IReadOnlyList<CheckDto>>
{
    public async ValueTask<IReadOnlyList<CheckDto>> Handle(GetChecks query, CancellationToken cancellationToken)
    {
        var checks = await getReceiptsService.GetChecksAsync(query);
        return checks.Select(check => check.ConvertToCheckList()).ToList();
    }
}
