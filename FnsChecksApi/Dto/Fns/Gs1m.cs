namespace FnsChecksApi.Dto.Fns;

public record Gs1m(
    string Gtin,
    string Sernum,
    int ProductIdType,
    string RawProductCode
);