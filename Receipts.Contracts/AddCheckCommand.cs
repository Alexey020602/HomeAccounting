using Fns.Contracts;
using Mediator;

namespace Receipts.Contracts;

public sealed class AddCheckCommand: ICommand
{
    public required string Login { get; init; }
    public required ReceiptFiscalData FiscalData { get; init; }
}