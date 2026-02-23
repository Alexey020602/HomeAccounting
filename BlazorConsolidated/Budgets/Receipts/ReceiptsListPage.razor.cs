using ClientServerContracts.Api.Budgets;
using ClientServerContracts.Budgets.GetReceipts;
using ClientServerShared.Results;
using MaybeResults;
using Microsoft.AspNetCore.Components;
using BlazorConsolidated.Common;
using MudBlazor;
using MudBlazor.Extensions;

namespace BlazorConsolidated.Budgets.Receipts;

public sealed partial class ReceiptsListPage
{
    [Inject] public required IBudgetsApi BudgetsApi { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    private MudDataGrid<Receipt> grid = new ();

    private async Task<GridData<Receipt>> LoadReceiptsGrid(GridState<Receipt> state, CancellationToken cancellation)
    {
        var query = new GetReceiptsQueryParameters
        {
            Skip = state.Page * state.PageSize,
            Take = state.PageSize,
            Filter = state.FilterQuery,
            Sorting = state.SortingQuery
        };

        var result = await BudgetsApi.GetReceipts(BudgetId, query, cancellation).TryAsync();

        if (result is Some<GetReceiptsResponse> some)
        {
            var response = some.Value;
            return new GridData<Receipt>
            {
                Items = response.Receipts.Select(r => new Receipt(r)),
                TotalItems = response.TotalCount
            };
        }

        Snackbar.Add("Не удалось загрузить список чеков", Severity.Error);
        return new GridData<Receipt> { Items = [], TotalItems = 0 };
    }

    private async Task OpenAddReceiptDialog()
    {
        var parameters = new DialogParameters<AddReceiptDialog>
        {
            { x => x.BudgetId, BudgetId }
        };

        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true
        };

        var dialog = await DialogService.ShowAsync<AddReceiptDialog>("Добавление чека", parameters, options);
        var result = await dialog.Result;

        if (result is not null && !result.Canceled)
        {
            await grid.ReloadServerData();
        }
    }

    private sealed record Receipt(Guid Id, decimal Sum, DateTime PurchaseDate, DateTime CreatedAt, string PurchasePlace, ReceiptStatus Status)
    {
        public Receipt(ReceiptDto dto) : this(dto.Id, dto.Sum / 100.0m, dto.PurchaseDate.LocalDateTime, dto.CreatedAt.LocalDateTime, dto.PurchasePlace, dto.Status)
        {
        }
    }
}