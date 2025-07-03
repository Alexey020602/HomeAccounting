namespace Receipts.Contracts;

public static class CheckRequestsExtensions
{
    // public static CheckRawRequest RawRequest(this CheckRequest request) => RawRequest(request, request.S ?? "");
    // public static CheckRawRequest RawRequest(this CheckRequest rawRequest, string sum) =>
    //     new CheckRawRequest(rawRequest.RawCheck(sum));
    public static string RawCheck(this CheckRequest request, string format = "yyyyMMddTHHmm")
    {
        return $"t={request.T.ToString(format)}&s={request.S}&fn={request.Fn}&i={request.Fd}&fp={request.Fp}&n=1";
    }
}