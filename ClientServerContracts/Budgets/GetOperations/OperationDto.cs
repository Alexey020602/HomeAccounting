namespace ClientServerContracts.Budgets.GetOperations;

/// <summary>
/// DTO for operation (manual spending).
/// </summary>
public sealed record OperationDto(
    Guid Id,
    string Description,
    long Sum,
    DateTimeOffset PurchaseDate,
    DateTimeOffset AddedDate,
    Guid UserId,
    int? CategoryId);
