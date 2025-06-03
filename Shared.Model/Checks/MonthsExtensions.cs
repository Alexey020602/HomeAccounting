namespace Shared.Model.Checks;

public static class MonthsExtensions
{
    public static Months GetMonths(this DateTime dateTime) => (Months)dateTime.Month;
    public static Months GetCurrentMonth() => DateTime.UtcNow.GetMonth();

    public static string Description(this Months months) => months switch
    {
        Months.January => "Январь",
        Months.February => "Февраль",
        Months.March => "Март",
        Months.April => "Апрель",
        Months.May => "Май",
        Months.June => "Июнь",
        Months.July => "Июль",
        Months.August => "Август",
        Months.September => "Сентябрь",
        Months.October => "Октябрь",
        Months.November => "Ноябрь",
        Months.December => "Декабрь",
        _ => "Неопределен",
    };
}