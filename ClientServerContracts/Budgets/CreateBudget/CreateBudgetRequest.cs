namespace ClientServerContracts.Budgets.CreateBudget;

public record CreateBudgetRequest(string Name, int? Limit, int BeginOfPeriod);