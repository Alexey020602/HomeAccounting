namespace HomeAccounting.Budgets.Data;

internal sealed class ReceiptProcessingOutboxEntry
{
    public Guid Id { get; private set; }
    public ReceiptId ReceiptId { get; private set; }
    public DateTimeOffset? NextRetryAt { get; private set; }
    public int AttemptCount { get; private set; }
    public string? LastErrorMessage { get; private set; }
    public ReceiptProcessingOutboxStatus Status { get; private set; }

    private ReceiptProcessingOutboxEntry() { }

    public static ReceiptProcessingOutboxEntry Create(ReceiptId receiptId)
    {
        return new ReceiptProcessingOutboxEntry
        {
            Id = Guid.NewGuid(),
            ReceiptId = receiptId,
            AttemptCount = 0,
            Status = ReceiptProcessingOutboxStatus.Pending
        };
    }

    public void MarkCompleted()
    {
        Status = ReceiptProcessingOutboxStatus.Completed;
        NextRetryAt = null;
        LastErrorMessage = null;
    }

    public void MarkFailed()
    {
        Status = ReceiptProcessingOutboxStatus.Failed;
        NextRetryAt = null;
    }

    public void ScheduleRetry(DateTimeOffset nextRetryAt, string errorMessage)
    {
        AttemptCount++;
        NextRetryAt = nextRetryAt;
        LastErrorMessage = errorMessage;
        Status = ReceiptProcessingOutboxStatus.Pending;
    }
}
