using FnsChecksApi.Dto.Fns;

namespace FnsChecksApi.Dto;

public record BadAnswerReceipt(int Code, string Data, Request Request): Receipt(Code, Request);