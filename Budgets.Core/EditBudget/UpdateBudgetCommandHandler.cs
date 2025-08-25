using Budgets.Core.GetBudgetDetail;
using Budgets.Core.UserInBudgetPermissions;
using MaybeResults;
using Shared.Utils.MediatorWithResults;

namespace Budgets.Core.EditBudget;

public sealed class UpdateBudgetCommandHandler(IUpdateBudgetService updateBudgetService) : IResultCommandHandler<UpdateBudgetCommand>
{
    public async ValueTask<IMaybe> Handle(UpdateBudgetCommand command, CancellationToken cancellationToken) =>
        await updateBudgetService.UpdateBudget(
            new()
            {
                Id = command.BudgetId,
                Name = command.BudgetData.Name,
                BeginOfPeriod = command.BudgetData.BeginOfPeriod,
                Limit = command.BudgetData.Limit
            }, 
            cancellationToken
        );
}