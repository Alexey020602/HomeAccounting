namespace HomeAccounting.ReceiptProcessing.GetReceiptData.Dto.Response;

public record ReceiptItem(
    // int Nds,
    int Sum,
    string Name,
    int Price,
    double Quantity
    // int PaymentType,
    // int ProductType,
    // ProductCodeNew ProductCodeNew,
    // int LabelCodeProcesMode,
    // IReadOnlyList<ItemsIndustryDetail> ItemsIndustryDetails,
    // int ItemsQuantityMeasure,
    // int CheckingProdInformationResult
);