namespace Accounting.Contracts;

public interface IAccountingSettingsService
{
    Task<int> GetFirstDayOfAccountingPeriod();
}