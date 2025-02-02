using Core.Extensions;

namespace Core.Model.Requests;

public class CheckRequest
{
    public CheckRequest(Dictionary<string, string> values, DateTimeOffset addedDate)
    {
        Fd = values["i"];
        Fn = values["fn"];
        Fp = values["fp"];
        S = values["s"];
        var dateTime = new DateTimeFnsParser().Parse(values["t"]).RemoveSeconds().ToUniversalTime();

        T = dateTime;
        AddedDate = addedDate.DateTime;
    }

    public CheckRequest(RawCheckRequest request) : this(CreateDictionaryFromRawString(request.QrRaw), request.AddedTime)
    {
    }

    public string Fn { get; init; }
    public string Fd { get; init; }
    public string Fp { get; init; }
    public string S { get; init; }
    public DateTime T { get; init; }
    public DateTime AddedDate { get; set; }

    private static Dictionary<string, string> CreateDictionaryFromRawString(string raw, char splitter = '&')
    {
        return raw
            .Split(splitter)
            .Select(str => str.Split('='))
            .Where(keyAndValue => keyAndValue.Length == 2)
            .Select(keyAndValue => new { Key = keyAndValue[0], Value = keyAndValue[1] })
            .ToDictionary(t => t.Key, t => t.Value);
    }
}