using Xunit;
using Moq;
using Microsoft.Extensions.Configuration;
using ChuckNorris.Core.Models;
using ChuckNorris.Core.Services;
using ChuckNorris.Core.Services.Transformations;

public class BadWordsFilterTests
{
    [Fact]
    public async Task TransformJokeAsync_BadWord_ReturnsNullAsync()
    {
        // Arranging the test
        var mockConfigSection = new Mock<IConfigurationSection>();
        var badWordsFilter = new BadWordsFilter(mockConfigSection.Object);
        badWordsFilter.BadWords.Add("badword");
        var joke = new JokeModel
        {
            Value = "This is a joke with a badword"
        };

        // Acting
        var result = await badWordsFilter.TransformJokeAsync(joke);

        // Asserting
        Assert.Null(result);
    }

    [Fact]
    public async Task TransformJokeAsync_NoBadWord_ReturnsNonNullOrEmptyAsync()
    {
        // Arranging the test
        var mockConfigSection = new Mock<IConfigurationSection>();
        var badWordsFilter = new BadWordsFilter(mockConfigSection.Object);
        badWordsFilter.BadWords.Add("badword");
        var joke = new JokeModel
        {
            Value = "This is a harmless joke without bad words"
        };

        // Acting
        var result = await badWordsFilter.TransformJokeAsync(joke);

        // Asserting
        Assert.NotNull(result);
        Assert.Equal(joke.Value, result.Value);
    }
}
