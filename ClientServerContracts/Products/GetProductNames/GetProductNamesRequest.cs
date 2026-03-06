namespace ClientServerContracts.Products.GetProductNames;

/// <summary>
/// Request parameters for getting unique product names list.
/// </summary>
/// <param name="Filter">
/// Filter string for filtering by product name. Format: FieldName[Operator]=Value or FieldName=Value (default operator is Eq).
/// Multiple filters are separated by commas.
/// <para>
/// Available operators:
/// <list type="bullet">
/// <item><term>eq</term><description>Equality</description></item>
/// <item><term>ne</term><description>Inequality</description></item>
/// <item><term>cn</term><description>Contains (string fields only)</description></item>
/// <item><term>nc</term><description>Does not contain (string fields only)</description></item>
/// <item><term>sw</term><description>Starts with (string fields only)</description></item>
/// <item><term>ew</term><description>Ends with (string fields only)</description></item>
/// </list>
/// </para>
/// <para>
/// Available fields (ProductField):
/// <list type="bullet">
/// <item><term>Name</term><description>Product name</description></item>
/// </list>
/// </para>
/// <para>
/// Examples:
/// <list type="bullet">
/// <item><term>Name[cn]=Молоко</term><description>Name contains "Молоко"</description></item>
/// <item><term>Name[sw]=Хлеб</term><description>Name starts with "Хлеб"</description></item>
/// </list>
/// </para>
/// </param>
/// <param name="Sorting">
/// Sorting string for ordering. Format: FieldName[Order] or FieldName (default order is Asc).
/// Multiple sortings are separated by commas.
/// <para>
/// Available orders:
/// <list type="bullet">
/// <item><term>Asc</term><description>Ascending order</description></item>
/// <item><term>Desc</term><description>Descending order</description></item>
/// </list>
/// </para>
/// <para>
/// Available fields (ProductSortField):
/// <list type="bullet">
/// <item><term>Name</term><description>Product name</description></item>
/// </list>
/// </para>
/// <para>
/// Examples:
/// <list type="bullet">
/// <item><term>Name[Asc]</term><description>Sort by name ascending</description></item>
/// <item><term>Name[Desc]</term><description>Sort by name descending</description></item>
/// </list>
/// </para>
/// </param>
/// <param name="Take">Number of items to take (pagination).</param>
/// <param name="Skip">Number of items to skip (pagination).</param>
public sealed record GetProductNamesRequest(
    string? Filter = null,
    string? Sorting = null,
    int? Take = null,
    int? Skip = null);
