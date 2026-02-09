namespace ClientServerContracts.Budgets.GetReceipts;

/// <summary>
/// Request parameters for getting receipts list.
/// </summary>
public sealed record GetReceiptsRequest(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    Guid? UserId = null,
    ReceiptStatus? Status = null,
    string? SortBy = null,
    bool? SortDescending = null,
    int? Take = null,
    int? Skip = null);
