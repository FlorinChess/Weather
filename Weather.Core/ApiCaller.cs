using System.Text.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Weather.Core
{
    public sealed class ApiCaller
    {
        public const string InvalidWeatherLocation = "Invalid weather location! Please enter a different location!";
        public const string ServerSideError = "Server-side error! Please try again later!";
        public const string ErrorOccured = "An error occured! Please try again!";

        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public ApiCaller(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public async ValueTask<WeatherInformation> GetWeather(string location, string lang = "en")
        {
            if (_cache.TryGetValue(location, out WeatherInformation weatherInformation))
            {
                return weatherInformation;
            }
            else
            {
                var response = await _httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key=592925aed75849868d1112324222302&q={location}&days=1&aqi=no&alerts=no&lang={lang}");

                if (response.IsSuccessStatusCode!)
                {
                    // Deserialize content object 
                    string content = await response.Content.ReadAsStringAsync();
                    var deserializedContent = JsonSerializer.Deserialize<WeatherInformation>(content);

                    _cache.Set(location, deserializedContent, TimeSpan.FromMinutes(15));
                    return deserializedContent;
                }
                else if ((int)response.StatusCode == 400)
                {
                    // Deserialize error object to get error code
                    string content = await response.Content.ReadAsStringAsync();
                    ApiError apiError = JsonSerializer.Deserialize<ApiError>(content);

                    // Error message that gets displayed depending on the error code
                    string errorMessage = ErrorOccured;

                    switch (apiError.error.code)
                    {
                        case 1006:
                            // Invalid weather location
                            errorMessage = InvalidWeatherLocation;
                            break;
                        case 9999:
                            // API internal error
                            errorMessage = ServerSideError;
                            break;
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
}
