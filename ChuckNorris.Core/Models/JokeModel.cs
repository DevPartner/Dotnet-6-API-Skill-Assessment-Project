using ChuckNorris.Core.Converters;
using System.Text.Json.Serialization;

namespace ChuckNorris.Core.Models;
public class JokeModel
{
    [JsonPropertyName("categories")]
    public List<string> Categories { get; set; } = new List<string>();

    [JsonPropertyName("created_at")]
    [JsonConverter(typeof(ChuckNorrisDateTimeConverter))]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; } = string.Empty;

    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("updated_at")]
    [JsonConverter(typeof(ChuckNorrisDateTimeConverter))]
    public DateTime UpdatedAt { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("value")]
    public string Value { get; set; } = string.Empty;
}