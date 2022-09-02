using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;

namespace Weather.Core.Tests
{
    [TestFixture]
    public class ApiCallerTest
    {
        private Mock<HttpMessageHandler> _httpMessageHandlerMock; 

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        }

        [Test]
        public async Task GetWeather_CorrectWeatherLocation_ReturnsWeatherInformation()
        {
            // Arrange

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[{ ""id"": 1, ""title"": ""Cool post!""}, { ""id"": 100, ""title"": ""Some title""}]"),
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                  "GetAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var apiCaller = SetupApiCaller(_httpMessageHandlerMock.Object);

            string testLocation = "New York";

            // Act
            var apiRespone = await apiCaller.GetWeather(testLocation);

            // Assert
            Assert.That(apiRespone, Is.InstanceOf<WeatherInformation>());
        }

        [Test]
        public async Task GetWeather_IncorrectWeatherLocation_ThrowsLocationNotFoundException()
        {
            // Arrange
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"[{ ""id"": 1, ""title"": ""Cool post!""}, { ""id"": 100, ""title"": ""Some title""}]"),
            };

            _httpMessageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var apiCaller = SetupApiCaller(_httpMessageHandlerMock.Object);

            string testLocation = "NotARealLocation";

            // Act
            var exception = Assert.ThrowsAsync<LocationNotFoundException>(async () => await apiCaller.GetWeather(testLocation));

            // Assert
            Assert.That(exception.Message, Is.EqualTo(ApiCaller.InvalidWeatherLocation));
        }

        private static ApiCaller SetupApiCaller(HttpMessageHandler httpMessageHandler)
        {
            return new ApiCaller(new HttpClient(httpMessageHandler));
        }
    }
}
