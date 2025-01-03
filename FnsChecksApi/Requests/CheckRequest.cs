using Refit;

namespace FnsChecksApi.Requests;

public class CheckRequest
{
    [AliasAs("token")] public string Token { get; set; } = "15239.20dUQQYmlHxbOPLzb";
    [AliasAs("fn")]
    public required string Fn { get; set; }
    [AliasAs("fd")]
    public required string Fd { get; set; }
    [AliasAs("fp")]
    public required string Fp { get; set; }
}