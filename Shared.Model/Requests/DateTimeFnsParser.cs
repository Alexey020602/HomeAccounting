using System.Globalization;

namespace Shared.Model.Requests;

public sealed class DateTimeFnsParser
{
    public string[] Formats =
    [
        "yyyyMMddTHHmmss",
        "yyyyMMddTHHmm"
    ];

    public DateTime Parse(string date)
    {
        foreach (var format in Formats)
            if (DateTime.TryParseExact(date, format, null, DateTimeStyles.AssumeUniversal, out var result))
                return result;

        throw new Exception("Cannot parse date string");
    }
}