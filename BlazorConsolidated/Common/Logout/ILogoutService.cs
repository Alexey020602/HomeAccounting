namespace BlazorConsolidated.Common.Logout;

public interface ILogoutService
{
    public Task Logout(CancellationToken cancellationToken = default);
}