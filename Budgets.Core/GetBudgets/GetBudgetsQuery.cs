using Budgets.Contracts.GetBudgets;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.GetBudgets;

public record GetBudgetsQuery(Guid UserId): IResultQuery<IReadOnlyCollection<Budget>>;