namespace MyBudgets.ReceiptProcessing.GetReceiptData.Dto.Response;

public record Gs1m(
    string Gtin,
    string Sernum,
    int ProductIdType,
    string RawProductCode
);