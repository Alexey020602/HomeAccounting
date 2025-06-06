using Authorization.Contracts;
using Checks.Core;
using Shared.Model;
using Shared.Model.Requests;

namespace Checks.Api.Requests;

internal static class CheckRequestExtensions
{
    public static SaveCheckRequest SaveCheckRequest(this CheckRequest request, string login) => new()
    {
        Login = login,
        Fn = request.Fn,
        Fd = request.Fd,
        Fp = request.Fp,
        S = request.S,
        T = request.T,
    };
}