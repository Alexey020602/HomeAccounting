namespace Fns.ReceiptData.ProverkaCheka.Dto;

public record Request(
    string Qrurl,
    string Qrfile,
    string Qrraw,
    Manual Manual
);