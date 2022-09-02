using System.Text.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;

namespace Weather.Core
{
    public class ApiCaller
    {
        public const string InvalidWeatherLocation = "Invalid weather location! Please enter a different location!";
        public const string ServerSideError = "Server-side error! Please try again later!";
        public const string ErrorOccured = "An error occured! Please try again!";

        private readonly HttpClient _httpClient;

        public ApiCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherInformation> GetWeather(string location, string lang = "en")
        {
            var response = await _httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key=592925aed75849868d1112324222302&q={location}&days=3&aqi=no&alerts=no&lang={lang}");

            if (response.IsSuccessStatusCode!)
            {
                // Deserialize content object 
                string content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<WeatherInformation>(content);
            }
            else if ((int)response.StatusCode == 400)
            {
                // Deserialize error object to get error code
                string content = await response.Content.ReadAsStringAsync();
                ApiError apiError = JsonSerializer.Deserialize<ApiError>(content);

                // Error message that gets displayed depending on the error code
                string errorMessage = ErrorOccured;

                if (apiError.error.code == 1006)
                {
                    // Invalid weather location
                    errorMessage = InvalidWeatherLocation;
                }
                else if (apiError.error.code == 9999)
                {
                    // API internal error
                    errorMessage = ServerSideError;
                }
                throw new LocationNotFoundException(errorMessage);
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
