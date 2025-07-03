using System.Text.Json.Serialization;

namespace Fns.Categorization.CheiCheck.Dto;

public class Product
{
    /// <summary>
    /// Ключевое (образующее) слово в названии продукта
    /// </summary>
    [JsonPropertyName("key_word")]
    public required string KeyWord { get; init; }
    /// <summary>
    /// Наименование продукта
    /// </summary>
    [JsonPropertyName("extended_description")]
    public required string? ExtendedDescription { get; init; }
}