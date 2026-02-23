namespace ClientServerContracts.Budgets.GetReceipts;

/// <summary>
/// Receipt processing status.
/// </summary>
public enum ReceiptStatus
{
    /// <summary>
    /// Receipt is being processed.
    /// </summary>
    Processing = 0,
    
    /// <summary>
    /// Receipt was successfully processed.
    /// </summary>
    Succeeded = 1,
    
    /// <summary>
    /// Receipt processing failed.
    /// </summary>
    Failed = 2
}
