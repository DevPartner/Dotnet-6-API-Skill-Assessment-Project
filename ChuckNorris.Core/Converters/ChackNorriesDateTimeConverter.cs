using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ChuckNorris.Core.Converters;

public class ChuckNorrisDateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.String)
        {
            throw new JsonException();
        }

        string dateString = reader.GetString();
        if (!DateTime.TryParseExact(dateString, "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
        {
            throw new JsonException();
        }

        return date;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
    }
}
