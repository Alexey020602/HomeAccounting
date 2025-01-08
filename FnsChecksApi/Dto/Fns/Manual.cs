using System.Text.Json.Serialization;

namespace FnsChecksApi.Dto.Fns;

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