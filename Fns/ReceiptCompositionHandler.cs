using Fns.Contracts;
using Mediator;

namespace Fns;

public sealed class ReceiptCompositionHandler() : IRequestHandler<ReceiptCompositionRequest, ReceiptCompositionResult>
{
    public ValueTask<ReceiptCompositionResult> Handle(ReceiptCompositionRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}