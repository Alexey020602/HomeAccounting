using Mediator;
using Receipts.Contracts;
using Receipts.Core.Mappers;

namespace Receipts.Core.GetReceipts;

public sealed class GetChecksHandler(IGetReceiptsService getReceiptsService): IQueryHandler<Contracts.GetChecks, IReadOnlyList<CheckDto>>
{
    public async ValueTask<IReadOnlyList<CheckDto>> Handle(Contracts.GetChecks query, CancellationToken cancellationToken)
    {
        var checks = await getReceiptsService.GetChecksAsync(query);
        return checks.Select(check => check.ConvertToCheckList()).ToList();
    }
}
