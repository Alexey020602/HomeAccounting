namespace FnsChecksApi.Dto.Fns;

public record Json(
    int Code,
    ///Организация
    string User,
    IReadOnlyList<Item> Items,
    int Nds10,
    string FnsUrl,
    string Region,
    ///ИНН Организации
    string UserInn,
    DateTime DateTime,
    string KktRegId,
    Metadata Metadata,
    string Operator,
    int TotalSum,
    int CreditSum,
    string NumberKkt,
    ///ФПД
    long FiscalSign,
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
    ///ФН
    string FiscalDriveNumber,
    double MessageFiscalSign,
    ///Адрес
    string RetailPlaceAddress,
    int AppliedTaxationType,
    ///ФД
    int FiscalDocumentNumber,
    int FiscalDocumentFormatVer,
    int CheckingLabeledProdResult
);