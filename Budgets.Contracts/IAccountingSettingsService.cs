namespace Budgets.Contracts;

public interface IAccountingSettingsService
{
    Task<int> GetFirstDayOfAccountingPeriod();
}