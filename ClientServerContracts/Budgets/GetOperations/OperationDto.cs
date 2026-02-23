namespace ClientServerContracts.Budgets.GetOperations;

/// <summary>
/// DTO for operation (manual spending).
/// </summary>
public sealed record OperationDto(
    Guid Id,
    string Description,
    string Sum,
    DateTimeOffset PurchaseDate,
    DateTimeOffset AddedDate,
    Guid UserId,
    int? CategoryId);
