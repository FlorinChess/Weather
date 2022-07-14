using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;

namespace Weather.Core.Tests
{
    [TestFixture]
    public class ApiCallerTest
    {
        [Test]
        public async Task GetWeather_CorrectWeatherLocation_ReturnsWeatherInformation()
        {
            // Arrange
            string testLocation = "New York";

            // Act
            var apiRespone = await ApiCaller.GetWeather(testLocation);

            // Assert
            Assert.That(apiRespone, Is.InstanceOf<WeatherInformation>());
        }

        [Test]
        public void GetWeather_IncorrectWeatherLocation_ThrowsLocationNotFoundException()
        {
            // Arrange
            string testLocation = "aleiraufheilafg";

            // Act
            var apiRespone = Assert.ThrowsAsync<LocationNotFoundException>(async () => await ApiCaller.GetWeather(testLocation));

            // Assert
            string expectedErrorMessage = "Invalid weather location! Please enter a different location!";
            Assert.That(apiRespone.Message, Is.EqualTo(expectedErrorMessage));
        }
    }
}
