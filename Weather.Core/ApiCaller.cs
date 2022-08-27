using System.Text.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Weather.Core.Exceptions;
using Weather.Core.Models;

namespace Weather.Core
{
    public static class ApiCaller
    {
        public static async Task<WeatherInformation> GetWeather(string location, string lang = "en")
        {
            using HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key=592925aed75849868d1112324222302&q={location}&days=3&aqi=no&alerts=no&lang={lang}");

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
                string errorMessage = "An error occured! Please try again!";

                if (apiError.error.code == 1006)
                {
                    // Invalid weather location
                    errorMessage = "Invalid weather location! Please enter a different location!";
                }
                else if (apiError.error.code == 9999)
                {
                    // API internal error
                    errorMessage = "Server-side error! Please try again later!";
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
