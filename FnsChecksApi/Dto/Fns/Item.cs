namespace FnsChecksApi.Dto.Fns;

public record Item(
    int Nds,
    int Sum,
    string Name,
    int Price,
    double Quantity,
    int PaymentType,
    int ProductType,
    ProductCodeNew ProductCodeNew,
    int LabelCodeProcesMode,
    IReadOnlyList<ItemsIndustryDetail> ItemsIndustryDetails,
    int ItemsQuantityMeasure,
    int CheckingProdInformationResult
);