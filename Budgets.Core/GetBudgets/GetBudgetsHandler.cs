using Budgets.Contracts.GetBudgets;
using MaybeResults;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.GetBudgets;

public sealed class GetBudgetsHandler(IGetBudgetsService budgetsService): IResultQueryHandler<GetBudgetsQuery, IReadOnlyCollection<Budget>>
{
    public ValueTask<IMaybe<IReadOnlyCollection<Budget>>> Handle(GetBudgetsQuery query,
        CancellationToken cancellationToken) =>
        new(budgetsService.GetBudgets(query));
}