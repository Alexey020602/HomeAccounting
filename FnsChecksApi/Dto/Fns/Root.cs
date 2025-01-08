namespace FnsChecksApi.Dto.Fns;

public record Root(
    int Code,
    int First,
    Data Data,
    Request Request
): Receipt(Code, Request);