namespace Fns.ReceiptData.ProverkaCheka.Dto;

public record Metadata(
    long Id,
    string OfdId,
    string Address,
    string Subtype,
    DateTime ReceiveDate
);