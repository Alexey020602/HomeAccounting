using ClientServerContracts.Api.Attributes;
using ClientServerContracts.Budgets;
using ClientServerContracts.Budgets.AddManualSpending;
using ClientServerContracts.Budgets.AddReceiptSpending;
using ClientServerContracts.Budgets.ChangeReceiptProductCategory;
using ClientServerContracts.Budgets.AddUsersInBudget;
using ClientServerContracts.Budgets.CreateBudget;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using ClientServerContracts.Budgets.GetBudgets;
using ClientServerContracts.Budgets.UserInBudgetPermissions;
using Refit;

namespace ClientServerContracts.Api.Budgets;

/// <summary>
/// Query parameters for GetBudgets endpoint.
/// </summary>
public sealed class GetBudgetsQueryParameters
{
}

/// <summary>
/// Query parameters for GetBudgetUsers endpoint.
/// </summary>
public sealed class GetBudgetUsersQueryParameters
{
}

/// <summary>
/// Query parameters for GetBudgetSpendings endpoint.
/// </summary>
public sealed class GetBudgetSpendingsQueryParameters
{
}

/// <summary>
/// Refit client for Budgets API endpoints.
/// </summary>
[ApiAuthorizable("budgets")]
[Headers("Authorization: Bearer")]
public interface IBudgetsApi
{
    /// <summary>
    /// Returns all budgets the authenticated user is a member of.
    /// </summary>
    /// <param name="query">Query parameters.</param>
    /// <returns>Collection of budgets.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved budgets.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request parameters.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Get("/")]
    public Task<IReadOnlyCollection<BudgetDto>> GetBudgets([Query] GetBudgetsQueryParameters query);

    /// <summary>
    /// Creates a new budget. The authenticated user becomes the owner.
    /// </summary>
    /// <param name="request">Budget creation request.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - Budget successfully created.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request data.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Post("/")] 
    public Task CreateBudget(CreateBudgetRequest request);

    /// <summary>
    /// Returns budget details by id. Requires read permission on the budget.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="query">Query parameters.</param>
    /// <returns>Budget details.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved budget details.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to read this budget.</description></item>
    /// </list>
    /// </remarks>
    [Get("/{id}")]
    public Task<BudgetDetailDto> GetBudgetDetail(Guid id);

    /// <summary>
    /// Updates budget by id. Requires edit permission on the budget.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="budgetData">Budget data to update.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>204</term><description>No Content - Budget successfully updated.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request data.</description></item>
    /// </list>
    /// </remarks>
    [Put("/{id}")]
    public Task UpdateBudget(Guid id, BudgetData budgetData);

    /// <summary>
    /// Deletes budget by id. Requires delete permission on the budget.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>204</term><description>No Content - Budget successfully deleted.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to delete this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request.</description></item>
    /// </list>
    /// </remarks>
    [Delete("/{id}")]
    public Task DeleteBudget(Guid id);

    /// <summary>
    /// Returns list of users in the budget with their roles. Requires read permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="query">Query parameters.</param>
    /// <returns>Collection of budget users.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved budget users.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to read this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request.</description></item>
    /// </list>
    /// </remarks>
    [Get("/{id}/users")]
    public Task<IReadOnlyCollection<BudgetUserDto>> GetBudgetUsers(Guid id, [Query] GetBudgetUsersQueryParameters query);

    /// <summary>
    /// Adds a user to the budget with the specified role. Requires edit permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="request">Request with user id and role id.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - User successfully added to budget.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request data (user not found, user already added, role not found).</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Post("/{id}/users")]
    public Task AddUsersInBudget(Guid id, AddUsersInBudgetRequest request);

    /// <summary>
    /// Removes a user from the budget. Requires edit permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="userId">User identifier to remove.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>204</term><description>No Content - User successfully removed from budget.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request (e.g., cannot remove owner).</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Delete("/{id}/users/{userId}")]
    public Task DeleteUserFromBudget(Guid id, Guid userId);

    /// <summary>
    /// Returns the current user's permissions (edit, delete) for the budget. Requires read permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="query">Query parameters.</param>
    /// <returns>User permissions in budget.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved user permissions.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to read this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request.</description></item>
    /// </list>
    /// </remarks>
    [Get("/{id}/permissions")]
    public Task<UserInBudgetPermissionsDto> GetUserPermissions(Guid id);

    /// <summary>
    /// Returns list of spendings in the budget. Requires read permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="query">Query parameters.</param>
    /// <returns>Budget spendings response.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved budget spendings.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to read this budget.</description></item>
    /// </list>
    /// </remarks>
    [Get("/{id}/spendings")]
    public Task<GetBudgetSpendingsResponse> GetBudgetSpendings(Guid id, [Query] GetBudgetSpendingsQueryParameters query);

    /// <summary>
    /// Adds spending from a receipt by fiscal data. Receipt is processed asynchronously. Requires edit permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="request">Receipt spending request with fiscal data.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - Receipt spending successfully added.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>409</term><description>Conflict - Receipt with these fiscal data already exists in this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request data.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Post("/{id}/receipts")]
    public Task AddReceiptSpending(Guid id, AddReceiptSpendingRequest request);

    /// <summary>
    /// Adds a manual spending entry to the budget. Requires edit permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="request">Manual spending request.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - Manual spending successfully added.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request data.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Post("/{id}/spendings/manual")]
    public Task AddManualSpending(Guid id, AddManualSpendingRequest request);

    /// <summary>
    /// Sets category for a product in a receipt. Requires edit permission.
    /// </summary>
    /// <param name="budgetId">Budget identifier.</param>
    /// <param name="receiptId">Receipt identifier.</param>
    /// <param name="productId">Product identifier.</param>
    /// <param name="request">Request with category id.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>204</term><description>No Content - Category set.</description></item>
    /// <item><term>404</term><description>Not Found - Budget or receipt not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Put("/{budgetId}/receipts/{receiptId}/products/{productId}/category")]
    public Task ChangeReceiptProductCategory(Guid budgetId, Guid receiptId, Guid productId, ChangeReceiptProductCategoryRequest request);

    /// <summary>
    /// Removes category from a product in a receipt. Requires edit permission.
    /// </summary>
    /// <param name="budgetId">Budget identifier.</param>
    /// <param name="receiptId">Receipt identifier.</param>
    /// <param name="productId">Product identifier.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>204</term><description>No Content - Category removed.</description></item>
    /// <item><term>404</term><description>Not Found - Budget or receipt not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Delete("/{budgetId}/receipts/{receiptId}/products/{productId}/category")]
    public Task DeleteReceiptProductCategory(Guid budgetId, Guid receiptId, Guid productId);
}
