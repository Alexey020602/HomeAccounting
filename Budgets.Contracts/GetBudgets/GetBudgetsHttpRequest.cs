using System.Text.Json.Serialization;

namespace Budgets.Contracts.GetBudgets;

[method: JsonConstructor]
public record GetBudgetsHttpRequest();