using Shared.Model.Dates;

namespace Receipts.Contracts;

public class CheckRequest(string Fn, string Fd, string Fp, string S, DateTime T/*, DateTime AddedDate*/)
{
    public CheckRequest(string rawCheckValue, DateTimeOffset addedDate, char splitter = '&'): this(CreateDictionaryFromRawString(rawCheckValue, splitter), addedDate)
    {
        
    }
    public CheckRequest(Dictionary<string, string> values, DateTimeOffset addedDate): 
        this(
            values["fn"], 
            values["i"], 
            values["fp"],
            values["s"], 
            new DateTimeFnsParser().Parse(values["t"]).RemoveSeconds().ToUniversalTime() 
            /*addedDate.DateTime*/)
    {
        
    }
    public string Fn { get; init; } = Fn;
    public string Fd { get; init; } = Fd;
    public string Fp { get; init; } = Fp;
    public string S { get; init; } = S;
    public DateTime T { get; init; } = T;
    // public DateTime AddedDate { get; set; } = AddedDate;

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