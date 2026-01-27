using HomeAccounting.Budgets.Data;
using HomeAccounting.Common.Infrastructure.Events;

namespace HomeAccounting.Budgets.Events;

internal sealed record ReceiptCreated(SpendingId ReceiptSpendingId) : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredDateTime { get; } = DateTimeOffset.UtcNow;
}
