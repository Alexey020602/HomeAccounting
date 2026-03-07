using MudBlazor;

namespace BlazorConsolidated;

internal record MenuItem(string Name, string Description, string Href, string Icon)
{
    private static readonly MenuItem ReceiptsList = new(
        "Чеки",
        "Просмотр чеков, добавленных в бюджет",
        RoutesConstants.Receipts,
        Icons.Material.Filled.List
    );

    private static readonly MenuItem OperationsList = new(
        "Операции",
        "Просмотр операций (ручных трат), добавленных в бюджет",
        RoutesConstants.Operations,
        Icons.Material.Filled.ShoppingCart
    );

    private static readonly MenuItem Categories = new(
        "Категории",
        "Просмотр категорий приложения",
        RoutesConstants.Categories,
        Icons.Material.Filled.Category
        );

    private static readonly MenuItem Products = new(
        "Товары",
        "Список уникальных наименований товаров",
        RoutesConstants.Products,
        Icons.Material.Filled.Inventory
        );

    public static IReadOnlyList<MenuItem> AuthorizedUserMenu =>
    [
        ReceiptsList,
        OperationsList,
        ..PublicItems
    ];

    public static IReadOnlyList<MenuItem> PublicItems =>
    [
        Categories,
        Products
    ];
}