namespace HomeAccounting.Budgets.Data;

internal sealed class ReceiptProcessingAttempt
{
    private ReceiptProcessingAttempt()
    {
    }

    private ReceiptProcessingAttempt(DateTime attemptedAt, bool isSuccess, string? errorMessage, int? errorCode)
    {
        AttemptedAt = attemptedAt;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorCode = errorCode;
    }

    public Guid Id { get; private set; }
    public DateTime AttemptedAt { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }
    public int? ErrorCode { get; private set; }

    public static ReceiptProcessingAttempt Success(DateTime attemptedAt) => new(attemptedAt, true, null, null);
    public static ReceiptProcessingAttempt Failure(DateTime attemptedAt, string errorMessage, int errorCode) =>
        new(attemptedAt, false, errorMessage, errorCode);
}