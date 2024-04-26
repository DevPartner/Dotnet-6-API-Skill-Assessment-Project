using Moq;
using Xunit;
using FluentAssertions;
using ChuckNorris.Core.Models;
using ChuckNorris.Core.Services;
using ChuckNorris.Core.Services.Transformations;

namespace ChuckNorris.Core.Tests;

public class JokeHandlerTests
{
    private readonly Mock<IChackNorriesConnector> chackNorriesConnectorMock;
    private readonly Mock<IJokeTransformation> jokeTransformationMock;
    private readonly JokeHandler jokeHandler;

    public JokeHandlerTests()
    {
        chackNorriesConnectorMock = new Mock<IChackNorriesConnector>();
        jokeTransformationMock = new Mock<IJokeTransformation>();
        jokeHandler = new JokeHandler(chackNorriesConnectorMock.Object, new List<IJokeTransformation> { jokeTransformationMock.Object });
    }

    [Fact]
    public async Task GetTransformedJokesAsync_ShouldReturnTransformedJokes()
    {
        // Arrange
        var joke = new JokeModel();
        var count = 5;

        chackNorriesConnectorMock.Setup(s => s.GetRandomJokeAsync(CancellationToken.None)).ReturnsAsync(joke);
        jokeTransformationMock.Setup(s => s.TransformJokeAsync(joke, CancellationToken.None)).ReturnsAsync(joke);

        // Act
        var transformedJokes = await jokeHandler.GetTransformedJokesAsync(count);

        // Assert
        transformedJokes.Should().HaveCount(count);
        foreach (var transformedJoke in transformedJokes)
        {
            transformedJoke.Should().Be(joke);
        }
    }

    [Fact]
    public async Task TransformJokeAsync_ShouldReturnTransformedJoke()
    {
        // Arrange
        var joke = new JokeModel();
        jokeTransformationMock.Setup(s => s.TransformJokeAsync(joke, CancellationToken.None)).ReturnsAsync(joke);

        // Act
        var transformedJoke = await jokeHandler.TransformJokeAsync(joke, CancellationToken.None);

        // Assert
        transformedJoke.Should().Be(joke);
    }
}
