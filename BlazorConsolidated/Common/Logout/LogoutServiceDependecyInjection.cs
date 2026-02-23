using Microsoft.Extensions.DependencyInjection;

namespace BlazorConsolidated.Common.Logout;

public static class LogoutServiceDependecyInjection
{
    public static IServiceCollection AddDefaultLogoutService(this IServiceCollection services) => services.AddScoped<ILogoutService, DefaultLogoutService>();
}