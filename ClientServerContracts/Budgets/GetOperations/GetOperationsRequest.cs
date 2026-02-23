
namespace ClientServerContracts.Budgets.GetOperations;

/// <summary>
/// Request parameters for getting operations list.
/// </summary>
public sealed record GetOperationsRequest(
    DateTimeOffset? StartDate = null,
    DateTimeOffset? EndDate = null,
    int? CategoryId = null,
    Guid? UserId = null,
    string? SortBy = null,
    bool? SortDescending = null,
    int? Take = null,
    int? Skip = null);
