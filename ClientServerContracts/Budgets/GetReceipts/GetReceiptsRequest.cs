namespace ClientServerContracts.Budgets.GetReceipts;

/// <summary>
/// Request parameters for getting receipts list.
/// </summary>
/// <param name="Filter">
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
/// </param>
/// <param name="Sorting">
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
/// </param>
/// <param name="Take">Number of receipts to take (pagination).</param>
/// <param name="Skip">Number of receipts to skip (pagination).</param>
public sealed record GetReceiptsRequest(
    // DateTimeOffset? StartDate = null,
    // DateTimeOffset? EndDate = null,
    // Guid? UserId = null,
    // ReceiptStatus? Status = null,
    // string? SortBy = null,
    // bool? SortDescending = null,
    string? Filter = null,
    string? Sorting = null,
    int? Take = null,
    int? Skip = null);


public record PagingRequest(string? Filter = null, string? Sort = null, int? Skip = null,  int? Take = null);