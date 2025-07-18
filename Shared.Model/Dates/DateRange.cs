using System.Diagnostics.CodeAnalysis;

namespace Shared.Model.Dates;

public readonly record struct DateRange(DateTime? Start = null, DateTime? End = null)
{
    public static DateRange CreateMonthRange(int firstDay, int month, int year) =>
        CreateMonthRangeFromDateTime(new DateTime(year, month, firstDay));

    public static DateRange CreateMonthRangeFromDateTime(DateTime start) => new(start, start.AddMonths(1));

    public bool Contains(DateTime date)
    {
        return !(date < Start || date > End);
    }
    public bool IsSameYear => Start?.Year == End?.Year;
    public bool IsSameMonth => IsSameYear && Start?.Month == End?.Month;
    public bool IsSameDay => IsSameMonth && Start?.Day == End?.Day;
    public bool IsSameHour => IsSameDay && Start?.Hour == End?.Hour;
    public override string ToString() => ToString(null, null);
    public string ToString([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format) => ToString(format, null);
    public string ToString([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format, IFormatProvider? provider)
    {
        switch (Start)
        {
            case null when End is null:
                return "Не ограничен";
            case null when End is not null:
                return $"До {EndString(format, provider)}";
            default:
            {
                if (Start is not null && End is null)
                {
                    return $"После {StartString(format, provider)}";
                }
                else 
                {
                    return $"С {StartString(StartStringFormat(format), provider)} до {EndString(format, provider)}";
                }
            }
        }
    }

    private string? StartStringFormat([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format)
    {
        if (format is null)
            return format;
        if (IsSameYear)
        {
            format = format.Replace("y", string.Empty);
        }

        if (IsSameMonth)
        {
            format = format.Replace("M", string.Empty);
            format = format.Replace("m", string.Empty);
        }

        if (IsSameDay)
        {
            format = format.Replace("d", string.Empty);
        }

        if (IsSameHour)
        {
            format = format.Replace("h", string.Empty);
            format = format.Replace("H", string.Empty);
        }
        
        return format;
    }

    private string? StartString([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format,
        IFormatProvider? provider) => Start?.ToString(format, provider);
    private string? EndString([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string? format,
        IFormatProvider? provider) => End?.ToString(format, provider);
}