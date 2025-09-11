using Microsoft.Extensions.DependencyInjection;

namespace Shared.Blazor.Logout;

public interface ILogoutService
{
    public Task Logout(CancellationToken cancellationToken = default);
}

public static class LogoutServiceDependecyInjection
{
    public static IServiceCollection AddDefaultLogoutService(this IServiceCollection services) => services.AddScoped<ILogoutService, DefaultLogoutService>();
}