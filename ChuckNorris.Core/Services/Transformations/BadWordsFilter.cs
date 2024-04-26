using ChuckNorris.Core.Models;
using Microsoft.Extensions.Configuration;

namespace ChuckNorris.Core.Services.Transformations;

public class BadWordsFilter : IJokeTransformation
{
    public HashSet<string> BadWords { get; set; } = new HashSet<string>();

    public BadWordsFilter(IConfigurationSection transformConfig)
    {
        transformConfig.Bind(this);
    }

    public Task<JokeModel> TransformJokeAsync(JokeModel joke, CancellationToken token = default)
    {
        if (joke == null) return Task.FromResult<JokeModel>(null!);

        return BadWords.Any(word => joke.Value.Contains(word, StringComparison.OrdinalIgnoreCase)) ?
               Task.FromResult<JokeModel>(null!) :
               Task.FromResult(joke);
    }
}
