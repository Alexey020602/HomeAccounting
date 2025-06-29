namespace Checks.Core;

public interface IReceiptSaveService
{
    public Task SaveReceipt(AddCheckRequest addCheckRequest);
}