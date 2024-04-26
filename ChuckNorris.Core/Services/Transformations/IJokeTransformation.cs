using ChuckNorris.Core.Models;

namespace ChuckNorris.Core.Services.Transformations;

public interface IJokeTransformation
{
    Task<JokeModel> TransformJokeAsync(JokeModel joke, CancellationToken token = default);
}
