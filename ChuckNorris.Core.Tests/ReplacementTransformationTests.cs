using ChuckNorris.Core.Models;
using ChuckNorris.Core.Models.Configs;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;
using FluentAssertions;
using ChuckNorris.Core.Services.Transformations;

namespace ChuckNorris.Core.Tests
{
    public class ReplacementTransformationTests
    {
        [Theory]
        [InlineData(" Chuck", " CH.")]
        [InlineData("Chuck ", "CH. ")]
        [InlineData(",Chuck", ",CH.")]
        [InlineData("Test Chuck Joke Chuck,", "Test CH. Joke CH.,")]
        public async Task TransformJoke_ReplacesCorrectly(string source, string expected)
        {
            // Arrange
            var testData = new List<ReplaceRuleConfig>
            {
                new ReplaceRuleConfig(){OldValue="Chuck", NewValue="CH." }
            };

            // Mock IConfiguration
            var mockConfig = new Mock<IConfigurationSection>();

            var transformation = new ReplacementTransformation(mockConfig.Object);
            transformation.ReplaceRules = testData;

            var joke = new JokeModel { Value = source };

            // Act
            var transformedJoke = await transformation.TransformJokeAsync(joke);

            // Assert
            transformedJoke.Value.Should().Be(expected);
        }
    }
}