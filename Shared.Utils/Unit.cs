using System.Text.Json;
using System.Text.Json.Serialization;

namespace Shared.Utils;

[Serializable]
public record Unit
{
    private Unit()
    {
    }
    public static readonly Unit Instance = new ();
}

public sealed class UnitJsonConverter : JsonConverter<Unit>
{
    public override Unit Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Если тело пустое или "{}", возвращаем Unit
        if (reader.TokenType != JsonTokenType.Null && reader.TokenType != JsonTokenType.None)
            throw new JsonException("Expected null or empty object for Unit");
        // Пропускаем объект, если это "{}"
        // if (reader.TokenType == JsonTokenType.StartObject)
        // {
        //     reader.Skip();
        // }
        return Unit.Instance;
    }

    public override void Write(Utf8JsonWriter writer, Unit value, JsonSerializerOptions options)
    {
        // Сериализуем Unit как пустой объект {}
        writer.WriteStartObject();
        writer.WriteEndObject();
    }
}