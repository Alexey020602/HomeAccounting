namespace Shared.Blazor.Logout;

public interface ILogoutAction
{
    public Task Logout(CancellationToken cancellationToken = default);
}