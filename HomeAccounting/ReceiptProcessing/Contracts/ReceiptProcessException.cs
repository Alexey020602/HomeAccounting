namespace HomeAccounting.ReceiptProcessing.Contracts;

public class ReceiptProcessException(string? message, Exception? innerException = null)
    : Exception(message, innerException);

internal sealed class IncorrectReceiptProcessException : ReceiptProcessException
{
    public IncorrectReceiptProcessException(string data)
        : base(data)
    {
    }
}

internal sealed class DataNotReceivedYetProcessException : ReceiptProcessException
{
    public DataNotReceivedYetProcessException(string data)
        : base(data)
    {
    }
}

internal sealed class NumberOfRequestsExceededProcessException : ReceiptProcessException
{
    public NumberOfRequestsExceededProcessException(string data)
        : base(data)
    {
    }
}

internal sealed class WaitingBeforeRepeatRequestProcessException : ReceiptProcessException
{
    public WaitingBeforeRepeatRequestProcessException(string data)
        : base(data)
    {
    }
}

internal sealed class OtherReceiptProcessException : ReceiptProcessException
{
    public OtherReceiptProcessException(string data)
        : base(data)
    {
    }
}

internal sealed class UnknownReceiptProcessException : ReceiptProcessException
{
    public UnknownReceiptProcessException(string message)
        : base(message)
    {
    }
}