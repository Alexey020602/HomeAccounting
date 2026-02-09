using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClientServerShared.Model;
using ClientServerShared.Model.Dates;
using ClientServerShared.Model.Money;

namespace HomeAccounting.Common.Model;

internal sealed record ReceiptFiscalData
{
    private ReceiptFiscalData(string fn, string fd, string fp, Money sum, DateTimeOffset purchaseDate)
    {
        Fn = fn;
        Fd = fd;
        Fp = fp;
        Sum = sum;
        PurchaseDate = purchaseDate;
    }

    /// <summary>
    /// Parameterless constructor for EF Core owned type materialization.
    /// </summary>
    private ReceiptFiscalData()
    {
        Fn = "0000000000000000";
        Fd = "000";
        Fp = "00000000";
        Sum = default;
        PurchaseDate = default;
    }

    public static ReceiptFiscalData Create(string fn, string fd, string fp, Money sum, DateTimeOffset purchaseDate)
    {
        if (fn.Length != 16)
        {
            throw new ArgumentException("fn length must be 16");
        }

        if (fd.Length is > 5 or < 3)
        {
            throw new ArgumentException("fd must be between 3 and 5");
        }

        if (fp.Length is > 10 or < 8)
        {
            throw new ArgumentException("fp must be between 10 and 8");
        } 
        
        
        if (!fn.All(char.IsDigit))
        {
            throw new ArgumentException("fn must be digit");
        }

        if (!fd.All(char.IsDigit))
        {
            throw new ArgumentException("fd must be digit");
        }

        if (!fp.All(char.IsDigit))
        {
            throw new ArgumentException("fp must be digit");
        }
        
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future");
        }
        
        return new ReceiptFiscalData(fn, fd, fp, sum, purchaseDate);
    }

    public static ReceiptFiscalData Empty()
    {
        // Возвращаем валидный объект с нулевыми значениями
        return new ReceiptFiscalData("0000000000000000", "000", "00000000", default, default);
    }

    /// <summary>
    /// Parses a QR code string in format: t=20240101T1200&s=12345&fn=1234567890123456&i=123&fp=12345678&n=1
    /// </summary>
    /// <param name="raw">Raw QR code string</param>
    /// <returns>Parsed ReceiptFiscalData</returns>
    /// <exception cref="ArgumentException">Thrown when parsing fails or validation fails</exception>
    public static ReceiptFiscalData Parse(string raw)
    {
        if (!TryParse(raw, out var result))
        {
            throw new ArgumentException($"Failed to parse receipt fiscal data from string: '{raw}'");
        }

        return result!;
    }

    /// <summary>
    /// Attempts to parse a QR code string in format: t=20240101T1200&s=12345&fn=1234567890123456&i=123&fp=12345678&n=1
    /// </summary>
    /// <param name="raw">Raw QR code string</param>
    /// <param name="result">Parsed ReceiptFiscalData if successful, null otherwise</param>
    /// <returns>True if parsing succeeded, false otherwise</returns>
    public static bool TryParse(string raw, out ReceiptFiscalData? result)
    {
        result = null;

        if (string.IsNullOrWhiteSpace(raw))
        {
            return false;
        }

        try
        {
            var values = CreateDictionaryFromRawString(raw);
            
            if (!values.TryGetValue("fn", out var fn) ||
                !values.TryGetValue("i", out var fd) ||
                !values.TryGetValue("fp", out var fp) ||
                !values.TryGetValue("s", out var s) ||
                !values.TryGetValue("t", out var t))
            {
                return false;
            }

            // Parse date
            var dateParser = new DateTimeFnsParser();
            var dateTime = dateParser.Parse(t).RemoveSeconds().ToUniversalTime();
            var purchaseDate = new DateTimeOffset(dateTime, TimeSpan.Zero);

            // Parse sum (in kopecks)
            if (!int.TryParse(s, out var sumKopecks))
            {
                return false;
            }

            var sum = Money.FromKopecks(sumKopecks);

            // Create ReceiptFiscalData with validation
            result = Create(fn, fd, fp, sum, purchaseDate);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static Dictionary<string, string> CreateDictionaryFromRawString(string raw, char splitter = '&')
    {
        return raw
            .Split(splitter)
            .Select(str => str.Split('='))
            .Where(keyAndValue => keyAndValue.Length == 2)
            .Select(keyAndValue => new { Key = keyAndValue[0], Value = keyAndValue[1] })
            .ToDictionary(t => t.Key, t => t.Value);
    }

    public string Fn { get; }
    public string Fd { get; }
    public string Fp { get; }
    public Money Sum { get; }
    public DateTimeOffset PurchaseDate { get; }
    public string Raw([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string format = "yyyyMMddTHHmm") => $"fn={Fn}&i={Fd}&fp={Fp}&t={PurchaseDate.ToString(format)}&s={Sum}&n=1";

    public void Deconstruct(out string fn, out string fd, out string fp, out Money sum, out DateTimeOffset purchaseDate)
    {
        fn = Fn;
        fd = Fd;
        fp = Fp;
        sum = Sum;
        purchaseDate = PurchaseDate;
    }
};

internal sealed record FullReceiptFiscalData
{
    public static FullReceiptFiscalData Create(Money sum, DateTimeOffset purchaseDate, ReceiptFiscalData fiscalData)
    {
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future");
        }
        
        return new FullReceiptFiscalData(sum, purchaseDate, fiscalData);
    }
    private FullReceiptFiscalData(Money Sum, DateTimeOffset PurchaseDate, ReceiptFiscalData FiscalData)
    {
        this.Sum = Sum;
        this.PurchaseDate = PurchaseDate;
        this.FiscalData = FiscalData;
    }
    public Money Sum { get; init; }
    public DateTimeOffset PurchaseDate { get; init; }
    public ReceiptFiscalData FiscalData { get; init; }
    public string GetRawString(string format = "yyyyMMddTHHmm") => $"{FiscalData.Raw(format)}&t={PurchaseDate.ToString(format)}&s={Sum}";
    public void Deconstruct(out Money sum, out DateTimeOffset purchaseDate, out ReceiptFiscalData fiscalData)
    {
        sum = Sum;
        purchaseDate = PurchaseDate;
        fiscalData = FiscalData;
    }
}