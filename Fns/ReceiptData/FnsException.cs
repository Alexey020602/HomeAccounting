using Fns.ReceiptData.ProverkaCheka.Dto;

namespace Fns.ReceiptData;

public class FnsException(string message) : Exception(message)
{
    internal static void ThrowIfInvalidResponse(Receipt receipt)
    {
        if (receipt is BadAnswerReceipt badAnswerReceipt) throw new FnsException($"{badAnswerReceipt.Data}");

        if (receipt is not Root) throw new FnsException("Invalid response type from FNS");
    }
}