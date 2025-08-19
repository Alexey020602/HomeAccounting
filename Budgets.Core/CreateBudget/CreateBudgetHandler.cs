using MaybeResults;
using Mediator;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.CreateBudget;

public sealed class CreateBudgetHandler(ICreateBudgetService createBudgetService): IResultCommandHandler<CreateBudgetCommand>
{
    public async ValueTask<IMaybe> Handle(CreateBudgetCommand command, CancellationToken cancellationToken)
    {
        return await createBudgetService.CreateAsync(command);
    }
}