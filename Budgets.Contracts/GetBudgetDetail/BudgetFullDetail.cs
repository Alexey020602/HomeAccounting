using System.Text.Json.Serialization;

namespace Budgets.Contracts.GetBudgetDetail;

[method: JsonConstructor]
public record BudgetFullDetail(Guid Id, string Name, int? Limit, int BeginOfPeriod, IReadOnlyCollection<BudgetUser> Users): BudgetDetail(Id, Name, Limit, BeginOfPeriod);

[method: JsonConstructor]
public record BudgetDetail(Guid Id, string Name, int? Limit, int BeginOfPeriod);
public record BudgetUser(Guid Id, string UserName, string Role);