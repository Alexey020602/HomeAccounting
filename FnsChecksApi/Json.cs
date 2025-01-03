using System.Text.Json.Serialization;

namespace FnsChecksApi;
public record Data(
    Json Json,
    string Html
);

public record Gs1m(
    string Gtin,
    string Sernum,
    int ProductIdType,
    string RawProductCode
);

public record Item(
    int Nds,
    int Sum,
    string Name,
    int Price,
    int Quantity,
    int PaymentType,
    int ProductType,
    ProductCodeNew ProductCodeNew,
    int LabelCodeProcesMode,
    IReadOnlyList<ItemsIndustryDetail> ItemsIndustryDetails,
    int ItemsQuantityMeasure,
    int CheckingProdInformationResult
);

public record ItemsIndustryDetail(
    string IdFoiv,
    string IndustryPropValue,
    string FoundationDocNumber,
    string FoundationDocDateTime
);

public record Json(
    int Code,
    string User,
    IReadOnlyList<Item> Items,
    int Nds10,
    string FnsUrl,
    string Region,
    string UserInn,
    DateTime DateTime,
    string KktRegId,
    Metadata Metadata,
    string Operator,
    int TotalSum,
    int CreditSum,
    string NumberKkt,
    int FiscalSign,
    int PrepaidSum,
    string OperatorInn,
    string RetailPlace,
    int ShiftNumber,
    int CashTotalSum,
    int ProvisionSum,
    int EcashTotalSum,
    string MachineNumber,
    int OperationType,
    int RedefineMask,
    int RequestNumber,
    string SellerAddress,
    string FiscalDriveNumber,
    double MessageFiscalSign,
    string RetailPlaceAddress,
    int AppliedTaxationType,
    int FiscalDocumentNumber,
    int FiscalDocumentFormatVer,
    int CheckingLabeledProdResult
);

public record Manual(
    string Fn,
    string Fd,
    string Fp,
    string CheckTime,
    string Type,
    string Sum
);

public record Metadata(
    long Id,
    string OfdId,
    string Address,
    string Subtype,
    DateTime ReceiveDate
);

public record ProductCodeNew(
    Gs1m Gs1m
);

public record Request(
    string Qrurl,
    string Qrfile,
    string Qrraw,
    Manual Manual
);

public record Root(
    int Code,
    int First,
    Data Data,
    Request Request
);

