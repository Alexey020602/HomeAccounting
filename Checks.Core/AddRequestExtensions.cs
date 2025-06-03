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
}