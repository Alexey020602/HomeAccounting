using System.Text.Json.Serialization;

namespace FnsChecksApi.Dto.Categorized;

public record Item(
    [property: JsonPropertyName("initial_request")]
    string InitialRequest,
    [property: JsonPropertyName("normalized_request")]
    string NormalizedRequest,
    string Brand,
    Product Product,
    IReadOnlyList<Measure> Measures,
    Category Category,
    double? Probability
);