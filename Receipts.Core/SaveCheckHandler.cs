using Rebus.Handlers;

namespace Receipts.Core;

public sealed class SaveReceiptRequest{}

public class SaveCheckHandler: IHandleMessages<SaveReceiptRequest>
{
    public Task Handle(SaveReceiptRequest message)
    {
        throw new NotImplementedException();
    }
}