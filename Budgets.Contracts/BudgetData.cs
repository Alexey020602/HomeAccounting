namespace Budgets.Contracts;

public record BudgetData(string Name, int BeginOfPeriod, int? Limit);