namespace Fns.ProverkaCheka.Dto;

public record BadAnswerReceipt(int Code, string Data, Request Request) : Receipt(Code, Request);