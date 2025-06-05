namespace Fns.Dto.Fns;

public record BadAnswerReceipt(int Code, string Data, Request Request) : Receipt(Code, Request);