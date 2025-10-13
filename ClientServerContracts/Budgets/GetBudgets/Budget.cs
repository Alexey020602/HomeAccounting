using System.Text.Json.Serialization;

namespace ClientServerContracts.Budgets.GetBudgets;

public sealed record Budget(Guid Id, string Name);