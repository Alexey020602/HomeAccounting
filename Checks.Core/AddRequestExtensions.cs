using Shared.Model.Requests;

namespace Checks.Core;

static class AddRequestExtensions
{
    public static GetCheckRequest CreateGetCheckRequestFromSaveCheckRequest(this SaveCheckRequest saveCheckRequest) =>
        new(
            saveCheckRequest.Fn,
            saveCheckRequest.Fd,
            saveCheckRequest.Fp,
            saveCheckRequest.S,
            saveCheckRequest.T);
    
    public static CheckRequest CreateCheckRequestFromSaveCheckRequest(this SaveCheckRequest saveCheckRequest) =>
        new(
            saveCheckRequest.Fn,
            saveCheckRequest.Fd,
            saveCheckRequest.Fp,
            saveCheckRequest.S,
            saveCheckRequest.T);
}