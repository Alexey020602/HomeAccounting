namespace HomeAccounting.Budgets.Data;

internal sealed class ReceiptProcessingAttempt
{
    private ReceiptProcessingAttempt()
    {
    }

    private ReceiptProcessingAttempt(DateTimeOffset attemptedAt, bool isSuccess, string? errorMessage)
    {
        AttemptedAt = attemptedAt;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public Guid Id { get; private set; }
    public DateTimeOffset AttemptedAt { get; private set; }
    public bool IsSuccess { get; private set; }
    public string? ErrorMessage { get; private set; }

    public static ReceiptProcessingAttempt Success(DateTimeOffset attemptedAt) => new(attemptedAt, true, null);
    public static ReceiptProcessingAttempt Failure(DateTimeOffset attemptedAt, string errorMessage) =>
        new(attemptedAt, false, errorMessage);
}