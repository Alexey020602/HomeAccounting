using System.Text.Json.Serialization;
using ClientServerShared;

namespace HomeAccounting.ReceiptProcessing.GetReceiptData.Dto.Response;

public record Manual(
    string Fn,
    string Fd,
    string Fp,
    string CheckTime,
    //todo подумать над переводом на [property: JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    [property: JsonConverter(typeof(IntOrStringJsonConverter))]
    string Type,
    string Sum
);