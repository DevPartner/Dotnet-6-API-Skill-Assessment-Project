using ChuckNorris.Core.Models;

namespace ChuckNorris.Core.Services
{
    public interface IJokeHandler
    {
        Task<IEnumerable<JokeModel>> GetTransformedJokesAsync(int count, CancellationToken cancellationToken = default);
    }
}