namespace Core.Services;

public interface IAccountingSettingsService
{
    Task<int> GetFirstDayOfAccountingPeriod();
}