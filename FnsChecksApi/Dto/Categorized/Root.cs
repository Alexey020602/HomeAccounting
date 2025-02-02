using System.Text.Json.Serialization;

namespace FnsChecksApi.Dto.Categorized;

public record Category(
    [property: JsonPropertyName("first_level_category")]
    string FirstLevelCategory,
    [property: JsonPropertyName("second_level_category")]
    string SecondLevelCategory
);

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

public record Measure(
    string Type,
    string Name,
    object Value
);

public record Product(
    [property: JsonPropertyName("key_word")]
    string KeyWord,
    [property: JsonPropertyName("full_description")]
    string FullDescription
);

public record Root(
    Status Status,
    IReadOnlyList<Item> Items
);

public record Status(
    bool Success,
    string? Message
);