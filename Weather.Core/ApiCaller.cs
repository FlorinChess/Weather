using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Weather.Core.Models;

namespace Weather.Core
{
    public class ApiCaller
    {
        public static async Task<WeatherInformation> GetWeather(string location, string? lang = "en")
        {
            using HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key=592925aed75849868d1112324222302&q={ location }&days=3&aqi=no&alerts=no&lang={ lang }");

            if (response.IsSuccessStatusCode!)
            {
                string content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<WeatherInformation>(content);
            }
            else
            {
                throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
