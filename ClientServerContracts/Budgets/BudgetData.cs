namespace ClientServerContracts.Budgets;

public record BudgetData(string Name, int BeginOfPeriod, int? Limit);