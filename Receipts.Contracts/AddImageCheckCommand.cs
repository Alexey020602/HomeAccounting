using Mediator;

namespace Receipts.Contracts;

public sealed class AddImageCheckCommand: ICommand
{
    public required Guid BudgetId { get; set; }
    public required Guid UserId { get; init; }
    public required byte[] ImageBytes { get; init; }
}