using System.IO;
using ClientServerContracts.Api.Budgets;
using ClientServerShared.Results;
using MaybeResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Refit;

namespace BlazorConsolidated.Budgets.Receipts;

public sealed partial class AddReceiptWithFileForm
{
    [Parameter] public required Guid BudgetId { get; set; }
    [Parameter] public EventCallback OnReceiptAdded { get; set; }
    
    [Inject] public required IBudgetsApi BudgetsApi { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    
    private MudForm form = null!;
    private IBrowserFile? SelectedFile { get; set; }
    // private IReadOnlyList<IBrowserFile>? _selectedFiles;
    // private IReadOnlyList<IBrowserFile>? SelectedFiles 
    // { 
    //     get => _selectedFiles;
    //     set 
    //     {
    //         _selectedFiles = value;
    //         if (value != null && value.Count > 0)
    //         {
    //             ValidateFile(value[0]);
    //         }
    //         else
    //         {
    //             ValidationError = null;
    //         }
    //     }
    // }
    private string? ValidationError { get; set; }
    private bool IsLoading { get; set; }

    // private IBrowserFile? SelectedFile => SelectedFiles?.FirstOrDefault();

    private void ValidateFile(IBrowserFile file)
    {
        ValidationError = null;
        
        if (file == null)
        {
            return;
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
        var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(fileExtension))
        {
            ValidationError = "Поддерживаются только файлы изображений: JPG, JPEG, PNG";
            SelectedFile = null;
            return;
        }

        if (file.Size > 10 * 1024 * 1024)
        {
            ValidationError = "Размер файла не должен превышать 10 МБ";
            SelectedFile = null;
            return;
        }
    }

    private void RemoveFile()
    {
        SelectedFile = null;
        ValidationError = null;
    }

    private async Task Submit()
    {
        if (SelectedFile == null)
        {
            Snackbar.Add("Выберите файл для загрузки", Severity.Warning);
            return;
        }

        IsLoading = true;
        ValidationError = null;
        
        try
        {
            await using var stream = SelectedFile.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
            // var streamPart = new StreamPart(stream, SelectedFile.Name, SelectedFile.ContentType);

            await BudgetsApi.AddReceiptFromFile(BudgetId, stream);
            
            Snackbar.Add("Чек успешно добавлен", Severity.Success);
            await OnReceiptAdded.InvokeAsync();
            SelectedFile = null;
            ValidationError = null;
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
}
