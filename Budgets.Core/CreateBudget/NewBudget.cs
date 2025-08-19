using System.ComponentModel.DataAnnotations;

namespace Budgets.Core.CreateBudget;

public record NewBudget(Guid UserId, string Name, int BeginOfPeriod, int? Limit, DateTime CreationDate)
{
    public static implicit operator NewBudget (CreateBudgetCommand command) => new(command.UserId, command.Name, command.BeginOfPeriod, command.Limit, command.CreationDate);
}