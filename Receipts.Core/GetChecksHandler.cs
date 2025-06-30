using Receipts.Contracts;
using Receipts.Core.Mappers;
using Mediator;

namespace Receipts.Core;

public sealed class GetChecksHandler(ICheckRepository checkRepository): IQueryHandler<GetChecks, IReadOnlyList<CheckDto>>
{
    public async ValueTask<IReadOnlyList<CheckDto>> Handle(GetChecks query, CancellationToken cancellationToken)
    {
        var checks = await checkRepository.GetChecksAsync((GetChecksQuery)query);
        return checks.Select(check => check.ConvertToCheckList()).ToList();
    }
}
