using Mediator;
using Receipts.Contracts;
using Shared.Utils.MediatorWithResults;

namespace Receipts.Core.AddReceipt;

public sealed class AddCheckCommand: IResultCommand
{
    public required ReceiptData ReceiptData { get; init; }
}