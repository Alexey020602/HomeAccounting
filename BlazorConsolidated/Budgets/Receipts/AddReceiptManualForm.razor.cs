using ClientServerContracts.Api.Budgets;
using ClientServerContracts.Budgets.AddReceiptSpending;
using ClientServerShared.Results;
using MaybeResults;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorConsolidated.Budgets.Receipts;

public sealed partial class AddReceiptManualForm
{
    [Parameter] public required Guid BudgetId { get; set; }
    [Parameter] public EventCallback OnReceiptAdded { get; set; }
    
    [Inject] public required IBudgetsApi BudgetsApi { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    private MudForm form = null!;
    private ManualInputModel Model { get; } = new();
    private bool IsLoading { get; set; }

    private async Task Submit()
    {
        await form.ValidateAsync();
        if (!form.IsValid)
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(Model.Fn) || 
            string.IsNullOrWhiteSpace(Model.Fd) || 
            string.IsNullOrWhiteSpace(Model.Fp) ||
            Model.Sum is not > 0 ||
            !Model.PurchaseDate.HasValue)
        {
            Snackbar.Add("Заполните все обязательные поля", Severity.Warning);
            return;
        }

        if (Model.PurchaseDate!.Value > DateTimeOffset.Now)
        {
            Snackbar.Add("Дата покупки не может быть в будущем", Severity.Warning);
            return;
        }

        IsLoading = true;
        try
        {
            var sumInKopecks = (int)(Model.Sum.Value * 100);
            var request = new AddReceiptSpendingRequest(
                Model.Fn,
                Model.Fd,
                Model.Fp,
                sumInKopecks,
                Model.PurchaseDate.Value);

            await BudgetsApi.AddReceiptSpending(BudgetId, request);

            Snackbar.Add("Чек успешно добавлен", Severity.Success);
            await OnReceiptAdded.InvokeAsync();
            Model.Reset();
            await form.ResetValidationAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ошибка при добавлении чека: {ex.Message}", Severity.Error);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private sealed class ManualInputModel
    {
        public string Fn { get; set; } = string.Empty;
        public string Fd { get; set; } = string.Empty;
        public string Fp { get; set; } = string.Empty;
        public decimal? Sum { get; set; }
        public DateTime? PurchaseDate { get; set; } = DateTime.Now;

        public void Reset()
        {
            Fn = string.Empty;
            Fd = string.Empty;
            Fp = string.Empty;
            Sum = null;
            PurchaseDate = DateTime.Now;
        }
    }
}
