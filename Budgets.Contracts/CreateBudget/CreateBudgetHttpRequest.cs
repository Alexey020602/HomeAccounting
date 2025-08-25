namespace Budgets.Contracts.CreateBudget;

public record CreateBudgetHttpRequest(string Name, int? Limit, int BeginOfPeriod);