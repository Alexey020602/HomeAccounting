using Fns.Contracts;
using Mediator;

namespace Receipts.Contracts;

public sealed class AddCheckCommand: ICommand
{
    public required ReceiptData ReceiptData { get; init; }
}