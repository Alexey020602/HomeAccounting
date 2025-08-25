using MaybeResults;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.DeleteBudget;

public interface IDeleteBudgetService
{
    Task<IMaybe> DeleteBudget(Guid budgetId);
}

public sealed class BudgetDeleteCommandHandler(IDeleteBudgetService deleteBudgetService): IResultCommandHandler<DeleteBudgetCommand>
{
    public ValueTask<IMaybe> Handle(DeleteBudgetCommand command, CancellationToken cancellationToken) => new(deleteBudgetService.DeleteBudget(command.BudgetId));
}