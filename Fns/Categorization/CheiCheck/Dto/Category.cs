using System.Text.Json.Serialization;

namespace Fns.Categorization.CheiCheck.Dto;

public record Category(
    [property: JsonPropertyName("first_level_category")]
    string FirstLevelCategory,
    [property: JsonPropertyName("second_level_category")]
    string? SecondLevelCategory
);