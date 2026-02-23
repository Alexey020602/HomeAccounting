using System.Globalization;

namespace HomeAccounting.Common.Model.ValueObjects;

public readonly record struct Money : IComparable<Money>
{
    public long Kopecks { get; }

    private Money(long kopecks) => Kopecks = kopecks;

    public static Money FromKopecks(long kopecks) => new(kopecks);

    public static Money FromRubles(decimal rubles)
    {
        var k = decimal.Round(rubles * 100m, 0, MidpointRounding.AwayFromZero);
        return new Money((long)k);
    }

    public static Money Zero => new Money(0);

    public decimal AmountRubles => Kopecks / 100m;

    public bool IsZero => Kopecks == 0;
    public bool IsNegative => Kopecks < 0;

    public Money Abs() => new(Math.Abs(Kopecks));

    public static Money operator +(Money a, Money b)
    {
        try
        {
            return new Money(checked(a.Kopecks + b.Kopecks));
        }
        catch (OverflowException)
        {
            throw new OverflowException($"Money addition overflow: {a.Kopecks} + {b.Kopecks}");
        }
    }

    public static Money operator -(Money a, Money b)
    {
        try
        {
            return new Money(checked(a.Kopecks - b.Kopecks));
        }
        catch (OverflowException)
        {
            throw new OverflowException($"Money subtraction overflow: {a.Kopecks} - {b.Kopecks}");
        }
    }

    public static Money operator -(Money a)
    {
        if (a.Kopecks == long.MinValue)
            throw new OverflowException("Cannot negate Money.MinValue");
        return new Money(-a.Kopecks);
    }
    
    public static bool operator >(Money a, Money b) => a.Kopecks > b.Kopecks;
    public static bool operator <(Money a, Money b) => a.Kopecks < b.Kopecks;
    public static bool operator >=(Money a, Money b) => a.Kopecks >= b.Kopecks;
    public static bool operator <=(Money a, Money b) => a.Kopecks <= b.Kopecks;

    public static Money operator *(Money a, decimal factor)
    {
        // важно: округляем до копейки
        var k = decimal.Round(a.Kopecks * factor, 0, MidpointRounding.AwayFromZero);
        
        // защита от переполнения long
        if (k < long.MinValue || k > long.MaxValue)
            throw new OverflowException($"Money multiplication overflow: {a.Kopecks} * {factor}");
        
        return new Money((long)k);
    }

    public static Money operator *(decimal factor, Money a) => a * factor;

    public static Money operator /(Money a, decimal divisor)
    {
        if (divisor == 0m) throw new DivideByZeroException();
        var k = decimal.Round(a.Kopecks / divisor, 0, MidpointRounding.AwayFromZero);
        return new Money((long)k);
    }

    public int CompareTo(Money other) => Kopecks.CompareTo(other.Kopecks);

    public override string ToString()
        => AmountRubles.ToString("0.00", CultureInfo.InvariantCulture);

    // ---------------------------
    // Parse / TryParse
    // ---------------------------

    /// <summary>
    /// Парсит строку вида "123.45" или "-10.00".
    /// Поддерживает также ввод без дробной части: "15" -> 15.00
    /// Округляет до копейки AwayFromZero при 3+ знаках после точки: "1.999" -> "2.00"
    /// </summary>
    public static Money Parse(string s)
    {
        if (!TryParse(s, out var result))
            throw new FormatException($"Invalid money format: '{s}'");

        return result;
    }

    /// <summary>
    /// Парсит строку вида "123.45" (точка как разделитель).
    /// Пробелы по краям допускаются.
    /// </summary>
    public static bool TryParse(string? s, out Money money)
    {
        money = default;

        if (string.IsNullOrWhiteSpace(s))
            return false;

        // Важно: InvariantCulture = точка как разделитель
        // AllowLeadingSign: допускаем минус/плюс
        // AllowDecimalPoint: допускаем дробную часть
        const NumberStyles styles = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint ;

        if (!decimal.TryParse(s.Trim(), styles, CultureInfo.InvariantCulture, out var rubles))
            return false;

        // Переводим в копейки и округляем до целого
        decimal kopecksDec = decimal.Round(rubles * 100m, 0, MidpointRounding.AwayFromZero);

        // защита от переполнения long
        if (kopecksDec < long.MinValue || kopecksDec > long.MaxValue)
            return false;

        money = new Money((long)kopecksDec);
        return true;
    }
}