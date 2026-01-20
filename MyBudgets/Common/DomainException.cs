namespace MyBudgets.Common;

internal sealed class DomainException(string? message = null, Exception? innerException = null): Exception(message, innerException);