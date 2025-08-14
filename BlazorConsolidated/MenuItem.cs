using MudBlazor;

namespace BlazorConsolidated;

internal record MenuItem(string Name, string Description, string Href, string Icon)
{
    public static readonly MenuItem AddReceipt = new(
        "Добавление чека",
        "Добавление нового чека в бюджет",
        RoutesConstants.ReceiptAdding,
        Icons.Material.Filled.Receipt
        );

    public static readonly MenuItem ReceiptsList = new(
        "Чеки",
        "Просмотр чеков, добавленных в бюджет",
        RoutesConstants.Receipts,
        Icons.Material.Filled.List
    );

    public static readonly MenuItem Report = new(
        "Отчет",
        "Просмотр отчета о текущем бюджете",
        RoutesConstants.Report,
        Icons.Material.Filled.Report
    );

    public static readonly IReadOnlyList<MenuItem> Menu =
    [
        AddReceipt,
        ReceiptsList,
        Report,
    ];
}