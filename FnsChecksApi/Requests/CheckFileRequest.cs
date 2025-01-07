using Refit;

namespace FnsChecksApi.Requests;

public class CheckFileRequest
{
    public string Token { get; set; } = "15239.20dUQQYmlHxbOPLzb";
    [AliasAs("qrfile")]
    public required FileInfo File { get; set; }

    public int Qr { get; } = 2;
}