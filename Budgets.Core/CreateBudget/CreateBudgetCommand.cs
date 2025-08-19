using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.CreateBudget;

public record CreateBudgetCommand(Guid UserId, string Name, int BeginOfPeriod, int? Limit, DateTime CreationDate): IResultCommand;