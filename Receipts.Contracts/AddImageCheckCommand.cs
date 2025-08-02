using Mediator;

namespace Receipts.Contracts;

public sealed class AddImageCheckCommand: ICommand
{
    public required Guid UserId { get; init; }
    public required byte[] ImageBytes { get; init; }
}