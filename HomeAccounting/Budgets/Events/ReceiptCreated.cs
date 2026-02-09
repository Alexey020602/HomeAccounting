using HomeAccounting.Budgets.Data;
using HomeAccounting.Common.Infrastructure.Events;

namespace HomeAccounting.Budgets.Events;

internal sealed record ReceiptCreated(ReceiptId ReceiptId) : IIntegrationEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTimeOffset OccurredDateTime { get; } = DateTimeOffset.UtcNow;
}
