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
    string? Filter = null,
    string? Sorting = null,
    int? Take = null,
    int? Skip = null);


public record PagingRequest(string? Filter = null, string? Sort = null, int? Skip = null,  int? Take = null);