using System.Text.Json.Serialization;
using Shared.Utils.Model.Dates;

namespace Receipts.Contracts;

[method: JsonConstructor]
public sealed record CheckRequest(string Fn, string Fd, string Fp, string S, DateTime T, string TimeZoneInfoId/*, DateTime AddedDate*/)
{
    public CheckRequest(
        string rawCheckValue, 
        string timeZoneInfo, 
        char splitter = '&'
        ): this(CreateDictionaryFromRawString(rawCheckValue, splitter), timeZoneInfo)
    {
        
    }
    public CheckRequest(Dictionary<string, string> values, string timeZoneInfoId): 
        this(
            values["fn"], 
            values["i"], 
            values["fp"],
            values["s"], 
            new DateTimeFnsParser().Parse(values["t"]).RemoveSeconds(),
            timeZoneInfoId
            /*addedDate.DateTime*/)
    {
        
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
}