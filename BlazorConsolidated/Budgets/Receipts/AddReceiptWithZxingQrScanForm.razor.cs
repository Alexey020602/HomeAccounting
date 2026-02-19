using ClientServerContracts.Api.Budgets;
using ClientServerContracts.Budgets.AddReceiptFromQrCode;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;
using ZXingBlazor;
using ZXingBlazor.Components;

namespace BlazorConsolidated.Budgets.Receipts;

public sealed partial class AddReceiptWithZxingQrScanForm
{
    [Parameter] public required Guid BudgetId { get; set; }
    [Parameter] public EventCallback OnReceiptAdded { get; set; }

    [Inject] public required IBudgetsApi BudgetsApi { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }

    private bool ShowScanner { get; set; }
    private bool IsLoading { get; set; }
    private ZXingOptions ZxingOptions = new ZXingOptions
    {
        Decodeonce = true,
        formats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }
    };

    private async Task OnQrCodeScanned(string scannedText)
    {
        Snackbar.Add($"Чек отсканирован {scannedText}", Severity.Success);
        if (string.IsNullOrWhiteSpace(scannedText) || IsLoading)
        {
            return;
        }

        IsLoading = true;
        StateHasChanged();

        try
        {
            await BudgetsApi.AddReceiptFromQrCode(BudgetId, new AddReceiptFromQrCodeRequest(scannedText));

            Snackbar.Add("Чек успешно добавлен", Severity.Success);
            await OnReceiptAdded.InvokeAsync();

            ShowScanner = false;
            StateHasChanged();
        }
        catch (ApiException ex)
        {
            Snackbar.Add($"Ошибка при добавлении чека: {ex.Message}", Severity.Error);
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ошибка при добавлении чека: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
            StateHasChanged();
        }
    }

    private Task OnCloseScanner()
    {
        ShowScanner = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private Task OnScanError(string message)
    {
        Snackbar.Add($"Ошибка сканирования: {message}", Severity.Warning);
        return Task.CompletedTask;
    }
}
