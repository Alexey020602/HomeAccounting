using System.Text.Json.Serialization;

namespace ClientServerContracts.Budgets.GetBudgets;

public sealed record BudgetDto(Guid Id, string Name);