namespace Fns.ProverkaCheka.Dto;

public record Gs1m(
    string Gtin,
    string Sernum,
    int ProductIdType,
    string RawProductCode
);