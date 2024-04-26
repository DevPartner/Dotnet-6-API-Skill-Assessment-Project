using ChuckNorris.Core.Converters;
using ChuckNorris.Core.Models;
using System.Text.Json;

namespace ChuckNorris.Core.Services;

public class ChackNorriesConnector : IChackNorriesConnector
{
    private readonly HttpClient _httpClient;
    public ChackNorriesConnector(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JokeModel> GetRandomJokeAsync(CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "random");
        using var response = await _httpClient.SendAsync(request, cancellationToken);

        response.EnsureSuccessStatusCode();

        using var responseStream = await response.Content.ReadAsStreamAsync();
        var joke = await JsonSerializer.DeserializeAsync<JokeModel>(responseStream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                // Add custom converters if needed
                Converters = { new ChuckNorrisDateTimeConverter() }
            }, cancellationToken);

        return joke!;
    }
}
