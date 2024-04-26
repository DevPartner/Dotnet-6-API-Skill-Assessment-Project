using ChuckNorris.Core.Models;
using ChuckNorris.Core.Services.Transformations;

namespace ChuckNorris.Core.Services
{
    public class JokeHandler : IJokeHandler
    {
        private readonly IChackNorriesConnector _chackNorriesConnector;
        private readonly IEnumerable<IJokeTransformation> _jokeTransformations;

        public JokeHandler(IChackNorriesConnector chackNorriesConnector, IEnumerable<IJokeTransformation> jokeTransformations)
        {
            _chackNorriesConnector = chackNorriesConnector;
            _jokeTransformations = jokeTransformations;
        }

        public async Task<IEnumerable<JokeModel>> GetTransformedJokesAsync(int count, CancellationToken cancellationToken = default)
        {
            var transformedJokeList = new List<JokeModel>();

            while (transformedJokeList.Count < count)
            {
                var tasks = Enumerable.Range(0, count - transformedJokeList.Count)
                    .Select(_ => _chackNorriesConnector.GetRandomJokeAsync(cancellationToken));

                var jokeList = await Task.WhenAll(tasks);

                var transformTasks = jokeList.Select(joke => TransformJokeAsync(joke, cancellationToken));
                var transformedJokes = await Task.WhenAll(transformTasks);

                transformedJokeList.AddRange(transformedJokes.Where(joke => joke != null));
            }

            return transformedJokeList;
        }

        public async Task<JokeModel> TransformJokeAsync(JokeModel joke, CancellationToken cancellationToken = default)
        {
            foreach (var jokeTransformation in _jokeTransformations)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                joke = await jokeTransformation.TransformJokeAsync(joke, cancellationToken);

                if (joke == null) // stop transformations
                    break;
            }

            return joke!;
        }
    }
}
