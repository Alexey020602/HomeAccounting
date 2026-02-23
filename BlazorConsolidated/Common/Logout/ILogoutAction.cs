namespace BlazorConsolidated.Common.Logout;

public interface ILogoutAction
{
    public Task Logout(CancellationToken cancellationToken = default);
}