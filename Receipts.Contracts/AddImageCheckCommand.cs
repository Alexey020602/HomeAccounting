using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Receipts.Contracts;

public sealed record AddImageCheckCommand(Guid BudgetId, Guid UserId, byte[] ImageBytes) : IResultCommand;