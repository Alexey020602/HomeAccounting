namespace ClientServerContracts.Budgets.GetBudgetDetail;

public record BudgetDetailDto(Guid Id, string Name, int? Limit, int BeginOfPeriod);