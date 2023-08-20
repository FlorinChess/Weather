using System.Text.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Net;

namespace Weather.Core
{
    public sealed class WeatherApiClient : IWeatherClient
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;

        public WeatherApiClient(HttpClient httpClient, IMemoryCache memoryCache)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
        }

        public async ValueTask<WeatherInformation> GetWeather(string location, string lang = "en")
        {
            if (_memoryCache.TryGetValue(location, out WeatherInformation weatherInformation))
            {
                return weatherInformation;
            }
            else
            {
                var response = await _httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key=592925aed75849868d1112324222302&q={location}&days=1&aqi=no&alerts=no&lang={lang}");

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize content object 
                    string content = await response.Content.ReadAsStringAsync();
                    var deserializedContent = JsonSerializer.Deserialize<WeatherInformation>(content);

                    _memoryCache.Set(location, deserializedContent, TimeSpan.FromMinutes(15));
                    return deserializedContent;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    // Deserialize error object to get error code
                    string content = await response.Content.ReadAsStringAsync();
                    ApiError apiError = JsonSerializer.Deserialize<ApiError>(content);

                    // Error message that gets displayed depending on the error code
                    throw apiError.error.code switch
                    {
                        1006 => new InvalidWeatherLocationException(),// Invalid weather location
                        9999 => new ServerSideIssueException(),// API internal error
                        _ => new ErrorOccuredException(),
                    };
                }
                else
                {
                    throw new Exception(response.StatusCode.ToString());
                }
            }
        }
    }
}
