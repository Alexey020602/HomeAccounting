using System.Text.Json.Serialization;

namespace FnsChecksApi.Dto.Categorized;

public record Product(
    [property: JsonPropertyName("key_word")]
    string KeyWord,
    [property: JsonPropertyName("full_description")]
    string FullDescription
);