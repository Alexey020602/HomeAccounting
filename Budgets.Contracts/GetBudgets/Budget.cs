using System.Text.Json.Serialization;

namespace Budgets.Contracts.GetBudgets;

[method: JsonConstructor]
public sealed class Budget(Guid id, string name)
{
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}