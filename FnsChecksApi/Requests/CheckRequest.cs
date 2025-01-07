using Refit;

namespace FnsChecksApi.Requests;

public class CheckRequest
{
    [AliasAs("token")] public string Token { get; set; } = "30787.hZOYna2xwcSHHqk3q";
    [AliasAs("fn")]
    public required string Fn { get; set; }
    [AliasAs("fd")]
    public required string Fd { get; set; }
    [AliasAs("fp")]
    public required string Fp { get; set; }
    public string? S  { get; set; }
    public string? T { get; set; }
    public int Qr { get; set; } = 0;
    public int N { get; set; } = 1;
}