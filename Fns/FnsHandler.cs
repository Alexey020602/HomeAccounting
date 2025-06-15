namespace Fns;

public record AddCheck
{
    public required string Fn { get; init; }
    public required string Fd { get; init; }
    public required string Fp { get; init; }
    public required string S { get; init; }
    public required DateTime T { get; init; }
    public required DateTime AddedDate { get; init; }
    public required string Login { get; init; }
}

public static class FnsHandler
{
    
}