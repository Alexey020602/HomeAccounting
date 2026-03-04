using BlazorConsolidated.Common;
using ClientServerContracts.Api.Products;
using ClientServerContracts.Products.GetProductNames;
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
    [Inject] public required IJSRuntime JSRuntime { get; set; }

    private MudDataGrid<ProductDto> _grid = new();
    private bool _csvDownloading;

    private async Task<GridData<ProductDto>> LoadProductsGrid(GridState<ProductDto> state, CancellationToken cancellation)
    {
        var query = new GetProductNamesQueryParameters
        {
            Skip = state.Page * state.PageSize,
            Take = state.PageSize,
            Filter = state.FilterQuery,
            Sorting = state.SortingQuery
        };

        var result = await ProductsApi.GetProductNames(query, cancellation).TryAsync();

        if (result is Some<GetProductNamesResponse> some)
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
            await using var stream = await ProductsApi.GetProductsCsv();
            using var memory = new MemoryStream();
            await stream.CopyToAsync(memory);
            var base64 = Convert.ToBase64String(memory.ToArray());
            await JSRuntime.InvokeAsync<object>("saveFileAs", "products.csv", "text/csv", base64);
        }
        finally
        {
            _csvDownloading = false;
            StateHasChanged();
        }
    }
}
