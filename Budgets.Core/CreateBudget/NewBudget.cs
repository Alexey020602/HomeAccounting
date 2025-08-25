using System.ComponentModel.DataAnnotations;
using Budgets.Contracts;

namespace Budgets.Core.CreateBudget;

public record NewBudget(Guid UserId, DateTime CreationDate, BudgetData BudgetData)
{
    public static implicit operator NewBudget (CreateBudgetCommand command) => new(command.UserId, command.CreationDate, new BudgetData( command.Name, command.BeginOfPeriod, command.Limit));
}