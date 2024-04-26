using ChuckNorris.Core.Services;
using Moq.Protected;
using Moq;
using System.Net;
using System.Text;
using Xunit;
using FluentAssertions;
using System;

namespace ChuckNorris.Core.Tests
{
    public class ChackNorriesConnectorTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private ChackNorriesConnector _chackNorriesConnector;

        public ChackNorriesConnectorTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();

            _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("https://api.chucknorris.io/jokes/")
            };

            _chackNorriesConnector = new ChackNorriesConnector(_httpClient);
        }

        [Fact]
        public async Task GetRandomJoke_Returns_Joke()
        {
            // Arrange
            var jokeResponse = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("{\"categories\":[],\"created_at\":\"2020-01-05 13:42:21.179347\",\"icon_url\":\"https://assets.chucknorris.host/img/avatar/chuck-norris.png\",\"id\":\"JEw6exwQT2mwQroELFbe5w\",\"updated_at\":\"2020-01-05 13:42:21.179347\",\"url\":\"https://api.chucknorris.io/jokes/JEw6exwQT2mwQroELFbe5w\",\"value\":\"Lightning never strikes Chuck Norris.If it does he gets an erection.\"}", Encoding.UTF8, "application/json")
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(jokeResponse);

            // Act
            var result = await _chackNorriesConnector.GetRandomJokeAsync();

            //Assert
            result.Should().NotBeNull();
            result.Id.Should().Be("JEw6exwQT2mwQroELFbe5w");
            result.IconUrl.Should().Be("https://assets.chucknorris.host/img/avatar/chuck-norris.png");
            result.Url.Should().Be("https://api.chucknorris.io/jokes/JEw6exwQT2mwQroELFbe5w");
            result.Value.Should().Be("Lightning never strikes Chuck Norris.If it does he gets an erection.");
            CheckDateTime(result.CreatedAt);
            CheckDateTime(result.UpdatedAt);
            result.Categories.Should().BeEmpty();

            static void CheckDateTime(DateTime dateTime)
            {
                dateTime.Year.Should().Be(2020);
                dateTime.Month.Should().Be(1);
                dateTime.Day.Should().Be(5);
                dateTime.Hour.Should().Be(13);
                dateTime.Minute.Should().Be(42);
                dateTime.Second.Should().Be(21);
                dateTime.Millisecond.Should().Be(179);
            }
        }
    }
}