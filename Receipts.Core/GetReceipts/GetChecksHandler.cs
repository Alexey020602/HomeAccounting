using Mediator;
using Receipts.Contracts;
using Receipts.Core.Mappers;
using Receipts.Core.Model;

namespace Receipts.Core.GetReceipts;

public sealed class GetChecksHandler(IGetReceiptsService getReceiptsService): IQueryHandler<GetChecks, IReadOnlyList<CheckDto>>
{
    public async ValueTask<IReadOnlyList<CheckDto>> Handle(GetChecks query, CancellationToken cancellationToken) => 
        (await getReceiptsService.GetChecksAsync(query))
        .Select(check => check.ConvertToCheckList())
        .ToList();
}
