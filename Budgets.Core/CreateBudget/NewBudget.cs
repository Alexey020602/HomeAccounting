using System.ComponentModel.DataAnnotations;

namespace Budgets.Core.CreateBudget;

public record BudgetData(string Name, int BeginOfPeriod, int? Limit);
public record NewBudget(Guid UserId, DateTime CreationDate, BudgetData BudgetData)
{
    public static implicit operator NewBudget (CreateBudgetCommand command) => new(command.UserId, command.CreationDate, new BudgetData( command.Name, command.BeginOfPeriod, command.Limit));
}