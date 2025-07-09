using Budgets.Contracts;
using Microsoft.Extensions.Configuration;

namespace Budgets.Core;
public sealed class ConfigurationAccountingSettingsService(IConfiguration configuration): IAccountingSettingsService
{
    public Task<int> GetFirstDayOfAccountingPeriod()
    {
        return Task.FromResult<int>(ConfigurationExtensions.GetSettings(configuration).PeriodStartDay);
    }
}