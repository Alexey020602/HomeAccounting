using HomeAccounting.Budgets.Data;

namespace HomeAccounting.Budgets.Services;

internal interface IReceiptProcessingOrchestrator
{
    Task ProcessEnableToRetries(CancellationToken cancellationToken);
    Task ProcessSpendingsByIds(SpendingId[] ids, CancellationToken cancellationToken);
}
