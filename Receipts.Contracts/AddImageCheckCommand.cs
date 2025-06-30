using Mediator;

namespace Receipts.Contracts;

public sealed class AddImageCheckCommand: ICommand
{
    public required string Login { get; init; }
    public required byte[] ImageBytes { get; init; }
}