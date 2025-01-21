using FnsChecksApi.Requests;

namespace FnsChecksApi;

public static class CheckRequestsExtensions
{
    public static CheckRawRequest RawRequest(this CheckRequest request) => RawRequest(request, request.S ?? "");
    public static CheckRawRequest RawRequest(this CheckRequest rawRequest, string sum) =>
        new CheckRawRequest(rawRequest.RawCheck(sum));
    static string RawCheck(this CheckRequest request, string sum) => $"t={request.T}&s={sum}&fn={request.Fn}&i={request.Fd}&fp={request.Fp}&n={request.N}";
}