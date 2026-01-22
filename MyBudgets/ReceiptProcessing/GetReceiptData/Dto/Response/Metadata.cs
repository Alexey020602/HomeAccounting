namespace MyBudgets.ReceiptProcessing.GetReceiptData.Dto.Response;

public record Metadata(
    long Id,
    string OfdId,
    string Address,
    string Subtype,
    DateTime ReceiveDate
);