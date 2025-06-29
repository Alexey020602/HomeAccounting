using System.Text.Json.Serialization;
using Shared.Model.Checks;
using Shared.Model.Requests;

namespace Fns.Contracts;

public record ReceiptFiscalData
{
    public ReceiptFiscalData(string raw): this(CreateDictionaryFromRawString(raw)) { }
    public string Fn { get; init; }
    public string Fd { get; init; }
    public string Fp { get; init; }
    public string S { get; init; }
    public DateTime T { get; init; }

    public string RawCheck(string format = "yyyyMMddTHHmm")
    {
        return $"t={T.ToString(format)}&s={S}&fn={Fn}&i={Fd}&fp={Fp}&n=1";
    }
    private ReceiptFiscalData(Dictionary<string, string> values) : this(
        values["fn"],
        values["i"],
        values["fp"],
        values["s"],
        new DateTimeFnsParser().Parse(values["t"]).RemoveSeconds().ToUniversalTime()) {}

    // public ReceiptFiscalData()
    // {
    // }

    [JsonConstructor]
    public ReceiptFiscalData( string Fn, string Fd, string Fp, string S, DateTime T)
    {
        this.Fn = Fn;
        this.Fd = Fd;
        this.Fp = Fp;
        this.S = S;
        this.T = T;
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

    public void Deconstruct(out string Fn, out string Fd, out string Fp, out string S, out DateTime T)
    {
        Fn = this.Fn;
        Fd = this.Fd;
        Fp = this.Fp;
        S = this.S;
        T = this.T;
    }
}