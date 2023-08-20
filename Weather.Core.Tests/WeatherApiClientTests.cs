using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.IO;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;

namespace Weather.Core.Tests
{
    [TestFixture]
    public sealed class WeatherApiClientTests
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        private WeatherApiClient InitializeSUT()
        {
            return new WeatherApiClient(new HttpClient(_httpMessageHandlerMock.Object), new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public async Task GetWeather_CorrectWeatherLocation_ReturnsWeatherInformation()
        {
            // Arrange
            var json = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "test_api_response.json"));

            var fakeResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(json),
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(fakeResponse);


            var weatherClient = InitializeSUT();

            string testLocation = "New York";

            // Act
            var apiRespone = await weatherClient.GetWeather(testLocation);

            // Assert
            Assert.That(apiRespone, Is.InstanceOf<WeatherInformation>());
        }

        [Test]
        public async Task GetWeather_IncorrectWeatherLocation_ThrowsLocationNotFoundException()
        {
            // Arrange
            var json = await File.ReadAllTextAsync(Path.Combine(Environment.CurrentDirectory, "test_error_response.json"));
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(json),
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var weatherClient = InitializeSUT();

            string testLocation = "NotARealLocation";

            // Act
            // Assert
            var exception = Assert.ThrowsAsync<InvalidWeatherLocationException>(async () => await weatherClient.GetWeather(testLocation));
        }
    }
}
