namespace ClientServerContracts.Budgets.GetReceipts;

/// <summary>
/// Response containing list of receipts.
/// </summary>
public sealed record GetReceiptsResponse(
    ReceiptDto[] Receipts,
    int TotalCount);
