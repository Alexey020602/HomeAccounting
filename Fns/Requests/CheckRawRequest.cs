using Refit;

namespace Fns.Requests;

public record CheckRawRequest
{
    public CheckRawRequest(string QrRaw, int Qr = 3, string Token = "30787.hZOYna2xwcSHHqk3q")
    {
        this.QrRaw = QrRaw;
        this.Qr = Qr;
        this.Token = Token;
    }

    [AliasAs("qrraw")] public string QrRaw { get; init; }

    [AliasAs("qr")] public int Qr { get; init; }

    [AliasAs("token")] public string Token { get; init; }

    public void Deconstruct(out string QrRaw, out int Qr, out string Token)
    {
        QrRaw = this.QrRaw;
        Qr = this.Qr;
        Token = this.Token;
    }
}