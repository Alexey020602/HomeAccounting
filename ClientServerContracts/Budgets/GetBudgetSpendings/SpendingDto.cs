namespace ClientServerContracts.Budgets.GetBudgetSpendings;

public sealed record SpendingDto(Guid Id, string Description, long Sum, Status Status);

public enum Status
{
    InProcess,
    Error, 
    Added,
}