namespace ClientServerContracts.Budgets.GetBudgetSpendings;

public sealed record SpendingDto(Guid Id, string Description, int Sum);