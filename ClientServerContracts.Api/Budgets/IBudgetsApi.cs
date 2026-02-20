using ClientServerContracts.Budgets;
using ClientServerContracts.Budgets.AddManualSpending;
using ClientServerContracts.Budgets.AddReceiptFromQrCode;
using ClientServerContracts.Budgets.AddReceiptSpending;
using ClientServerContracts.Budgets.ChangeReceiptProductCategory;
using ClientServerContracts.Budgets.AddUsersInBudget;
using ClientServerContracts.Budgets.CreateBudget;
using ClientServerContracts.Budgets.GetBudgetDetail;
using ClientServerContracts.Budgets.GetBudgetSpendings;
using ClientServerContracts.Budgets.GetBudgets;
using ClientServerContracts.Budgets.GetOperations;
using ClientServerContracts.Budgets.GetReceipts;
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
/// Query parameters for GetReceipts endpoint.
/// </summary>
public sealed class GetReceiptsQueryParameters
{
    /// <summary>
    /// Filter string for filtering receipts. Format: FieldName[Operator]=Value or FieldName=Value (default operator is Eq).
    /// Multiple filters are separated by commas.
    /// <para>
    /// Available operators:
    /// <list type="bullet">
    /// <item><term>eq</term><description>Equality</description></item>
    /// <item><term>ne</term><description>Inequality</description></item>
    /// <item><term>gt</term><description>Greater than</description></item>
    /// <item><term>gte</term><description>Greater than or equal</description></item>
    /// <item><term>lt</term><description>Less than</description></item>
    /// <item><term>lte</term><description>Less than or equal</description></item>
    /// <item><term>cn</term><description>Contains (string fields only)</description></item>
    /// <item><term>nc</term><description>Does not contain (string fields only)</description></item>
    /// <item><term>sw</term><description>Starts with (string fields only)</description></item>
    /// <item><term>ew</term><description>Ends with (string fields only)</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Available fields (ReceiptField):
    /// <list type="bullet">
    /// <item><term>PurchaseDate</term><description>Purchase date</description></item>
    /// <item><term>PurchasePlace</term><description>Purchase place</description></item>
    /// <item><term>UserId</term><description>User identifier</description></item>
    /// <item><term>BudgetId</term><description>Budget identifier</description></item>
    /// <item><term>Sum</term><description>Receipt sum</description></item>
    /// <item><term>Status</term><description>Processing status</description></item>
    /// <item><term>CreatedAt</term><description>Creation date</description></item>
    /// <item><term>CompletedAt</term><description>Completion date</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Examples:
    /// <list type="bullet">
    /// <item><term>PurchaseDate[gte]=2024-01-01</term><description>Purchase date >= 2024-01-01</description></item>
    /// <item><term>Status[eq]=Succeeded</term><description>Status equals Succeeded</description></item>
    /// <item><term>PurchasePlace[cn]=Магазин</term><description>Purchase place contains "Магазин"</description></item>
    /// <item><term>Sum[gt]=1000,PurchaseDate[lt]=2024-12-31</term><description>Multiple filters</description></item>
    /// </list>
    /// </para>
    /// </summary>
    [AliasAs("filter")]
    public string? Filter { get; set; }

    /// <summary>
    /// Sorting string for ordering receipts. Format: FieldName[Order] or FieldName (default order is Asc).
    /// Multiple sortings are separated by commas.
    /// <para>
    /// Available orders:
    /// <list type="bullet">
    /// <item><term>Asc</term><description>Ascending order</description></item>
    /// <item><term>Desc</term><description>Descending order</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Available fields (ReceiptSortField):
    /// <list type="bullet">
    /// <item><term>PurchaseDate</term><description>Purchase date</description></item>
    /// <item><term>Sum</term><description>Receipt sum</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Examples:
    /// <list type="bullet">
    /// <item><term>PurchaseDate[Desc]</term><description>Sort by purchase date descending</description></item>
    /// <item><term>Sum[Asc]</term><description>Sort by sum ascending</description></item>
    /// <item><term>PurchaseDate[Desc],Sum[Asc]</term><description>Multiple sortings</description></item>
    /// </list>
    /// </para>
    /// </summary>
    [AliasAs("sorting")]
    public string? Sorting { get; set; }

    /// <summary>
    /// Number of receipts to take (pagination).
    /// </summary>
    [AliasAs("take")]
    public int? Take { get; set; }

    /// <summary>
    /// Number of receipts to skip (pagination).
    /// </summary>
    [AliasAs("skip")]
    public int? Skip { get; set; }
}

/// <summary>
/// Query parameters for GetOperations endpoint.
/// </summary>
public sealed class GetOperationsQueryParameters
{
    /// <summary>
    /// Start date for filtering operations by purchase date.
    /// </summary>
    [AliasAs("startDate")]
    public DateTimeOffset? StartDate { get; set; }

    /// <summary>
    /// End date for filtering operations by purchase date.
    /// </summary>
    [AliasAs("endDate")]
    public DateTimeOffset? EndDate { get; set; }

    /// <summary>
    /// Category identifier for filtering operations by category.
    /// </summary>
    [AliasAs("categoryId")]
    public int? CategoryId { get; set; }

    /// <summary>
    /// User identifier for filtering operations by user.
    /// </summary>
    [AliasAs("userId")]
    public Guid? UserId { get; set; }

    /// <summary>
    /// Field name to sort by. Possible values: "PurchaseDate", "AddedDate", "Sum", "Description".
    /// </summary>
    [AliasAs("sortBy")]
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction. True for descending, false for ascending.
    /// </summary>
    [AliasAs("sortDescending")]
    public bool? SortDescending { get; set; }

    /// <summary>
    /// Number of operations to take (pagination).
    /// </summary>
    [AliasAs("take")]
    public int? Take { get; set; }

    /// <summary>
    /// Number of operations to skip (pagination).
    /// </summary>
    [AliasAs("skip")]
    public int? Skip { get; set; }
}

/// <summary>
/// Refit client for Budgets API endpoints.
/// </summary>
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
    [Get("/budgets")]
    public Task<IReadOnlyCollection<BudgetDto>> GetBudgets([Query] GetBudgetsQueryParameters query, CancellationToken cancellationToken = default);

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
    [Post("/budgets")] 
    public Task CreateBudget(CreateBudgetRequest request, CancellationToken cancellationToken = default);

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
    [Get("/budgets/{id}")]
    public Task<BudgetDetailDto> GetBudgetDetail(Guid id, CancellationToken cancellationToken = default);

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
    [Put("/budgets/{id}")]
    public Task UpdateBudget(Guid id, BudgetData budgetData, CancellationToken cancellationToken = default);

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
    [Delete("/budgets/{id}")]
    public Task DeleteBudget(Guid id, CancellationToken cancellationToken = default);

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
    [Get("/budgets/{id}/users")]
    public Task<IReadOnlyCollection<BudgetUserDto>> GetBudgetUsers(Guid id, [Query] GetBudgetUsersQueryParameters query, CancellationToken cancellationToken = default);

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
    [Post("/budgets/{id}/users")]
    public Task AddUsersInBudget(Guid id, AddUsersInBudgetRequest request, CancellationToken cancellationToken = default);

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
    [Delete("/budgets/{id}/users/{userId}")]
    public Task DeleteUserFromBudget(Guid id, Guid userId, CancellationToken cancellationToken = default);

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
    [Get("/budgets/{id}/permissions")]
    public Task<UserInBudgetPermissionsDto> GetUserPermissions(Guid id, CancellationToken cancellationToken = default);

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
    [Get("/budgets/{id}/spendings")]
    public Task<GetBudgetSpendingsResponse> GetBudgetSpendings(Guid id, [Query] GetBudgetSpendingsQueryParameters query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns list of receipts in the budget with optional filtering and sorting. Supports pagination. Requires read permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="query">Query parameters for filtering, sorting and pagination. Use Filter parameter for filtering by any receipt field with various operators. Use Sorting parameter for ordering by purchase date or sum.</param>
    /// <returns>Receipts response with list of receipts and total count.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved receipts.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to read this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request parameters (e.g., invalid filter or sorting format).</description></item>
    /// </list>
    /// </remarks>
    [Get("/budgets/{id}/receipts")]
    public Task<GetReceiptsResponse> GetReceipts(Guid id, [Query] GetReceiptsQueryParameters query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns list of operations (manual spendings) in the budget with optional filtering by date range, category, and user. Supports sorting and pagination. Requires read permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="query">Query parameters for filtering, sorting and pagination.</param>
    /// <returns>Operations response with list of operations and total count.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>200</term><description>OK - Successfully retrieved operations.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to read this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Invalid request parameters.</description></item>
    /// </list>
    /// </remarks>
    [Get("/budgets/{id}/operations")]
    public Task<GetOperationsResponse> GetOperations(Guid id, [Query] GetOperationsQueryParameters query, CancellationToken cancellationToken = default);

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
    [Post("/budgets/{id}/receipts")]
    public Task AddReceiptSpending(Guid id, AddReceiptSpendingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a receipt by uploading an image file with QR code. QR code is scanned, fiscal data is parsed, and receipt is processed asynchronously. Requires edit permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="file">Image file with QR code containing receipt fiscal data.</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - Receipt successfully added.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>409</term><description>Conflict - Receipt with these fiscal data already exists in this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Failed to read QR code from image, failed to parse receipt fiscal data, or error processing image file.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Post("/budgets/{id}/receipts/file")]
    [Multipart]
    public Task AddReceiptFromFile(Guid id, [AliasAs("File")] Stream file, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a receipt from raw QR code string. Fiscal data is parsed and receipt is processed asynchronously. Requires edit permission.
    /// </summary>
    /// <param name="id">Budget identifier.</param>
    /// <param name="request">Request with raw QR code string (FNS format: t=...&amp;s=...&amp;fn=...&amp;i=...&amp;fp=...&amp;n=1).</param>
    /// <returns>Task that completes when the request is finished.</returns>
    /// <exception cref="ApiException">Thrown on non-success status codes.</exception>
    /// <remarks>
    /// Possible status codes:
    /// <list type="table">
    /// <item><term>201</term><description>Created - Receipt successfully added.</description></item>
    /// <item><term>404</term><description>Not Found - Budget not found.</description></item>
    /// <item><term>403</term><description>Forbidden - User does not have permission to edit this budget.</description></item>
    /// <item><term>409</term><description>Conflict - Receipt with these fiscal data already exists in this budget.</description></item>
    /// <item><term>400</term><description>Bad Request - Failed to parse receipt fiscal data from QR code string.</description></item>
    /// <item><term>500</term><description>Internal Server Error - Server error occurred.</description></item>
    /// </list>
    /// </remarks>
    [Post("/budgets/{id}/receipts/qrcode")]
    public Task AddReceiptFromQrCode(Guid id, [Body] AddReceiptFromQrCodeRequest request, CancellationToken cancellationToken = default);

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
    [Post("/budgets/{id}/spendings/manual")]
    public Task AddManualSpending(Guid id, AddManualSpendingRequest request, CancellationToken cancellationToken = default);

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
    [Put("/budgets/{budgetId}/receipts/{receiptId}/products/{productId}/category")]
    public Task ChangeReceiptProductCategory(Guid budgetId, Guid receiptId, Guid productId, ChangeReceiptProductCategoryRequest request, CancellationToken cancellationToken = default);

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
    [Delete("/budgets/{budgetId}/receipts/{receiptId}/products/{productId}/category")]
    public Task DeleteReceiptProductCategory(Guid budgetId, Guid receiptId, Guid productId, CancellationToken cancellationToken = default);
}
