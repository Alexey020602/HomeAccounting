using System.Text.Json.Serialization;

namespace Budgets.Contracts.GetBudgetDetail;

[method: JsonConstructor]
public record BudgetDetail(Guid Id, string Name, int? Limit, int BeginOfPeriod);