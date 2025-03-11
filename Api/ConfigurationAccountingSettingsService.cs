using Core.Services;

namespace Api;
public sealed class ConfigurationAccountingSettingsService(IConfiguration configuration): IAccountingSettingsService
{
    public Task<int> GetFirstDayOfAccountingPeriod()
    {
        return Task.FromResult(configuration.GetSettings().PeriodStartDay);
    }
}