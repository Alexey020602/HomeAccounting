using HomeAccounting.Budgets.Data;

namespace HomeAccounting.Budgets.Services;

internal interface IReceiptProcessingOrchestrator
{
    Task ProcessEnableToRetries(CancellationToken cancellationToken);
    Task ProcessReceiptsByIds(ReceiptId[] ids, CancellationToken cancellationToken);
}
