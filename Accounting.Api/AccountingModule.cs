using Accounting.Contracts;
using Accounting.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Accounting.Api;

public static class AccountingModule
{
    public static IServiceCollection AddAccountingModule(this IServiceCollection services) =>
        services.AddSingleton<IAccountingSettingsService, ConfigurationAccountingSettingsService>();
}