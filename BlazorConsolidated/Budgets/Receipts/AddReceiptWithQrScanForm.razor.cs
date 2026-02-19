using BlazorQrCodeScanner;
using BlazorQrCodeScanner.MediaTrack;
using ClientServerContracts.Api.Budgets;
using ClientServerContracts.Budgets.AddReceiptFromQrCode;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Refit;

namespace BlazorConsolidated.Budgets.Receipts;

public sealed partial class AddReceiptWithQrScanForm : IAsyncDisposable
{
    [Parameter] public required Guid BudgetId { get; set; }
    [Parameter] public EventCallback OnReceiptAdded { get; set; }

    [Inject] public required IBudgetsApi BudgetsApi { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }

    private QrCodeScanner? qrScanner;
    private bool IsLoading { get; set; }

    private async Task OnScannerCreated()
    {
        if (qrScanner is null) return;
        await qrScanner.StartAsync(
            new MediaTrackConstraintSet { FacingMode = VideoFacingMode.Environment },
            new QrCodeConfig
            {
                FormatsToSupport = [BarcodeType.QR_CODE],
                Fps = 10
            });
    }

    private async Task OnQrCodeScanned(QrCodeScanResult result)
    {
        if (string.IsNullOrWhiteSpace(result?.DecodedText) || IsLoading)
        {
            return;
        }

        IsLoading = true;
        StateHasChanged();

        try
        {
            await BudgetsApi.AddReceiptFromQrCode(BudgetId, new AddReceiptFromQrCodeRequest(result.DecodedText));

            Snackbar.Add("Чек успешно добавлен", Severity.Success);
            await OnReceiptAdded.InvokeAsync();

            if (qrScanner != null)
            {
                await qrScanner.StopAsync();
            }
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

    private void OnScannerStartFailed()
    {
        Snackbar.Add("Не удалось запустить камеру. Проверьте разрешения.", Severity.Warning);
    }

    public async ValueTask DisposeAsync()
    {
        if (qrScanner != null)
        {
            await qrScanner.StopAsync();
        }
    }
}
