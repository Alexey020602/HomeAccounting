using System.Text.Json.Serialization;

namespace Budgets.Contracts.GetBudgets;

[method: JsonConstructor]
public sealed record Budget(Guid Id, string Name);