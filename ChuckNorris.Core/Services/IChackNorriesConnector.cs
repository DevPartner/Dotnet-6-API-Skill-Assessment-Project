using ChuckNorris.Core.Models;

namespace ChuckNorris.Core.Services;

public interface IChackNorriesConnector
{
    Task<JokeModel> GetRandomJokeAsync(CancellationToken cancellationToken = default);
}