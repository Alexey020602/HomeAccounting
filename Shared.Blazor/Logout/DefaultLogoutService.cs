namespace Shared.Blazor.Logout;

internal sealed class DefaultLogoutService(IEnumerable<ILogoutAction> logoutActions): ILogoutService
{
    public Task Logout(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        //todo Подумать над поведением при пустой коллекции действий
        if (logoutActions == null || !logoutActions.Any())
        {
            throw new InvalidOperationException("logoutActions cannot be null or empty.");
        }
        return Task.WhenAll(logoutActions.Select(x => x.Logout(cancellationToken)));
    }
}