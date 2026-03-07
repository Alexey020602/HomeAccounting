using BlazorConsolidated.Common;
using ClientServerContracts.Api.Products;
using ClientServerContracts.Budgets.GetProducts;
using ClientServerShared.Results;
using MaybeResults;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using MudBlazor.Extensions;

namespace BlazorConsolidated.Products;

public sealed partial class ProductsListPage
{
    [Inject] public required IProductsApi ProductsApi { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    // [Inject] public required IJSRuntime JSRuntime { get; set; }
    [Inject] public required IFileDownloader FileDownloader { get; set; }

    private MudDataGrid<ProductDto> _grid = new();
    private bool _csvDownloading;

    private async Task<GridData<ProductDto>> LoadProductsGrid(GridState<ProductDto> state, CancellationToken cancellation)
    {
        var query = new GetProductsQueryParameters
        {
            Skip = state.Page * state.PageSize,
            Take = state.PageSize,
            Filter = state.FilterQuery,
            Sorting = state.SortingQuery
        };

        var result = await ProductsApi.GetProducts(query, cancellation).TryAsync();

        if (result is Some<GetProductsResponse> some)
        {
            var response = some.Value;
            return new GridData<ProductDto>
            {
                Items = response.Items,
                TotalItems = response.TotalCount
            };
        }

        Snackbar.Add("Не удалось загрузить список товаров", Severity.Error);
        return new GridData<ProductDto> { Items = [], TotalItems = 0 };
    }

    private async Task DownloadCsvAsync()
    {
        if (_csvDownloading) return;
        _csvDownloading = true;
        try
        {
            await FileDownloader.DownloadFileAsync( "/api/products/csv");
        }
        finally
        {
            _csvDownloading = false;
            StateHasChanged();
        }
    }
}
