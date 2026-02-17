using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ClientServerShared.Model;
using ClientServerShared.Model.Dates;
using ClientServerShared.Model.Money;
using HomeAccounting.Budgets.Data;

namespace HomeAccounting.Common.Model;

/// <summary>
/// DTO для межмодульного взаимодействия с фискальными данными чека.
/// </summary>
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
    /// Creates ReceiptFiscalData from Value Objects (preferred method).
    /// </summary>
    public static ReceiptFiscalData Create(FiscalNumber fn, FiscalDocument fd, FiscalSign fp, Money sum, DateTimeOffset purchaseDate)
    {
        if (purchaseDate > DateTimeOffset.UtcNow)
        {
            throw new ArgumentException("You cannot add receipt from future", nameof(purchaseDate));
        }
        
        return new ReceiptFiscalData(fn.Value, fd.Value, fp.Value, sum, purchaseDate);
    }

    /// <summary>
    /// Creates ReceiptFiscalData from strings with validation (legacy method, creates Value Objects internally).
    /// </summary>
    public static ReceiptFiscalData Create(string fn, string fd, string fp, Money sum, DateTimeOffset purchaseDate)
    {
        var fiscalNumber = FiscalNumber.Create(fn);
        var fiscalDocument = FiscalDocument.Create(fd);
        var fiscalSign = FiscalSign.Create(fp);
        
        return Create(fiscalNumber, fiscalDocument, fiscalSign, sum, purchaseDate);
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

            // Create Value Objects with validation
            var fiscalNumber = FiscalNumber.Create(fn);
            var fiscalDocument = FiscalDocument.Create(fd);
            var fiscalSign = FiscalSign.Create(fp);
            
            // Create ReceiptFiscalData from Value Objects
            result = Create(fiscalNumber, fiscalDocument, fiscalSign, sum, purchaseDate);
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