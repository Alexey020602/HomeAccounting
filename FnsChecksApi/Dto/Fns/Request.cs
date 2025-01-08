namespace FnsChecksApi.Dto.Fns;

public record Request(
    string Qrurl,
    string Qrfile,
    string Qrraw,
    Manual Manual
);