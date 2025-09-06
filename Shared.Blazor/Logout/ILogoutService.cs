using Microsoft.Extensions.DependencyInjection;

namespace Shared.Blazor.Logout;

public interface ILogoutService
{
    public Task Logout(CancellationToken cancellationToken = default);
}

public interface ILogoutAction
{
    public Task Logout(CancellationToken cancellationToken = default);
}
sealed class DefaultLogoutService(IEnumerable<ILogoutAction> logoutActions): ILogoutService
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

public static class LogoutServiceDependecyInjection
{
    public static IServiceCollection AddDefaultLogoutService(this IServiceCollection services) => services.AddScoped<ILogoutService, DefaultLogoutService>();
}