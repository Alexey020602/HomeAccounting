using System.Text.Json.Serialization;

namespace Fns.Dto.Categorized;

public class Item
{
    [JsonPropertyName("initial_request")]
    public required string InitialRequest { get; init; }

    [JsonPropertyName("normalized_request")]
    public required string NormalizedRequest { get; init; }
    public required string Brand { get; init; }
    public required Product Product { get; init; }
    public required IReadOnlyList<Measure> Measures { get; init; }
    public required Category Category { get; init; }
    public double? Probability { get; init; }
}