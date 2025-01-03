using Refit;

namespace FnsChecksApi.Requests;

public class CheckStreamRequest
{
    public string Token { get; set; } = "15239.20dUQQYmlHxbOPLzb";
    [AliasAs("qrfile")]
    public required Stream Stream { get; set; }
}