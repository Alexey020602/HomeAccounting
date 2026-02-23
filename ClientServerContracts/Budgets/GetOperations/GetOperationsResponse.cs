namespace ClientServerContracts.Budgets.GetOperations;

/// <summary>
/// Response containing list of operations.
/// </summary>
public sealed record GetOperationsResponse(
    OperationDto[] Operations,
    int TotalCount);
