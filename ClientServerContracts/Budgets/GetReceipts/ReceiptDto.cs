namespace ClientServerContracts.Budgets.GetReceipts;

/// <summary>
/// DTO for receipt.
/// </summary>
public sealed record ReceiptDto(
    Guid Id,
    long Sum,
    DateTimeOffset PurchaseDate,
    DateTimeOffset CreatedAt,
    DateTimeOffset? CompletedAt,
    Guid UserId,
    string PurchasePlace,
    ReceiptStatus Status,
    string? LastErrorMessage,
    string Fn,
    string Fd,
    string Fp);
