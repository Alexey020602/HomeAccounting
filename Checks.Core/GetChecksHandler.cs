using Checks.Contracts;
using Checks.Core.Mappers;
using Mediator;

namespace Checks.Core;

public sealed class GetChecksHandler(ICheckRepository checkRepository): IQueryHandler<GetChecks, IReadOnlyList<CheckDto>>
{
    public async ValueTask<IReadOnlyList<CheckDto>> Handle(GetChecks query, CancellationToken cancellationToken)
    {
        var checks = await checkRepository.GetChecksAsync((GetChecksQuery)query);
        return checks.Select(check => check.ConvertToCheckList()).ToList();
    }
}
