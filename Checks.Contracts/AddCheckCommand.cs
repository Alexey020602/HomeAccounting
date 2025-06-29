using Fns.Contracts;
using Mediator;

namespace Checks.Contracts;

public sealed class AddCheckCommand: ICommand
{
    public required string Login { get; init; }
    public required ReceiptFiscalData FiscalData { get; init; }
}