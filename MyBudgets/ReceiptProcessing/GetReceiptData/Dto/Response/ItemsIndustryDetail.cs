namespace MyBudgets.ReceiptProcessing.GetReceiptData.Dto.Response;

public record ItemsIndustryDetail(
    string IdFoiv,
    string IndustryPropValue,
    string FoundationDocNumber,
    string FoundationDocDateTime
);